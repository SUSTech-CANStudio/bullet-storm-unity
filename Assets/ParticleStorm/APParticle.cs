using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ParticleStorm.Core;

namespace ParticleStorm
{
	/// <summary>
	/// Abstract particle system particle.
	/// </summary>
	public abstract class APParticle : MonoBehaviour, IParticle
	{
		public UpdateMode updateMode = UpdateMode.UPDATE;
		public ParticleSystemRenderMode renderMode
		{
			get => psr.renderMode;
			set { _renderMode = value; psr.renderMode = value; }
		}
		public Material[] materials
		{
			get => _materials;
			set { _materials = value; psr.materials = _materials; }
		}
		public Material material
		{
			get => psr.material;
			set { _materials[0] = value; psr.material = value; }
		}
		public Mesh[] meshes
		{
			get => _meshes;
			set { _meshes = value; psr.SetMeshes(_meshes); }
		}
		public Mesh mesh => psr.mesh;
		public ParticleSystem.TriggerModule trigger { get; private set; }
		public ParticleSystem.MainModule main { get; private set; }

		/// <summary>
		/// Called on start, configure the particle here. 
		/// </summary>
		public abstract void ParticleStart();
		/// <summary>
		/// Called every update.
		/// </summary>
		/// <param name="self">The particle itself.</param>
		public abstract void ParticleUpdate(ref ParticleSystem.Particle self);
		/// <summary>
		/// Called when enter collider.
		/// </summary>
		/// <param name="self">The particle itself.</param>
		public virtual void OnEnter(ParticleSystem.Particle self) { }
		/// <summary>
		/// Called when exit collider.
		/// </summary>
		/// <param name="self">The particle itself.</param>
		public virtual void OnExit(ParticleSystem.Particle self) { }
		/// <summary>
		/// Called when inside colleder.
		/// </summary>
		/// <param name="self">The particle itself.</param>
		public virtual void OnInside(ParticleSystem.Particle self) { }
		/// <summary>
		/// Called when outside colleder.
		/// </summary>
		/// <param name="self">The particle itself.</param>
		public virtual void OnOutside(ParticleSystem.Particle self) { }

		public void Emit(EmitParams emitParams, int num)
		{
			ps.Emit(emitParams.full, num);
		}

		protected void Start()
		{
			InitializeIfNeeded();
		}
		protected void Update()
		{
			if (updateMode == UpdateMode.UPDATE)
				ParticleUpdate();
		}
		protected void FixedUpdate()
		{
			if (updateMode == UpdateMode.FIXEDUPDATE)
				ParticleUpdate();
		}
		protected void LateUpdate()
		{
			if (updateMode == UpdateMode.LATEUPDATE)
				ParticleUpdate();
		}
		protected void OnParticleTrigger()
		{
			if (trigger.enter == ParticleSystemOverlapAction.Callback)
				ParticleTrigger(ParticleSystemTriggerEventType.Enter, OnEnter);

			if (trigger.exit == ParticleSystemOverlapAction.Callback)
				ParticleTrigger(ParticleSystemTriggerEventType.Exit, OnExit);

			if (trigger.inside == ParticleSystemOverlapAction.Callback)
				ParticleTrigger(ParticleSystemTriggerEventType.Inside, OnInside);

			if (trigger.outside == ParticleSystemOverlapAction.Callback)
				ParticleTrigger(ParticleSystemTriggerEventType.Outside, OnOutside);
		}

		delegate void OnTrigger(ParticleSystem.Particle self);

		void ParticleUpdate()
		{
			InitializeIfNeeded();
			int numAlive = ps.GetParticles(m_Particles);

			Parallel.For(0, numAlive, i =>
			{
				ParticleUpdate(ref m_Particles[i]);
			});

			ps.SetParticles(m_Particles, numAlive);
		}

		void ParticleTrigger(ParticleSystemTriggerEventType type, OnTrigger func)
		{
			List<ParticleSystem.Particle> trigerList = new List<ParticleSystem.Particle>();
			int numEnter = ps.GetTriggerParticles(type, trigerList);

			for(int i = 0; i < numEnter; i++)
			{
				func(trigerList[i]);
			}

			ps.SetTriggerParticles(type, trigerList);
		}

		void InitializeIfNeeded()
		{
			if (ps == null)
			{
				if (!TryGetComponent(out ps))
					ps = gameObject.AddComponent<ParticleSystem>();

				main = ps.main;
				psr = ps.GetComponent<ParticleSystemRenderer>();
				trigger = ps.trigger;

				// Disable auto generate
				ps.Stop();
				// Enable GPU
				psr.enableGPUInstancing = true;
				//Initial
				ps.transform.position = Vector3.zero;
				ps.transform.rotation = Quaternion.identity;
				ps.gameObject.isStatic = true;

				psr.renderMode = _renderMode;
				if (_materials == null || _materials.Length == 0)
					Debug.LogWarning("Material is null");
				else
					psr.materials = _materials;
				if (psr.renderMode == ParticleSystemRenderMode.Mesh)
					if(_meshes == null || _meshes.Length == 0)
						Debug.LogWarning("Mesh is null");
					else
					{
						psr.SetMeshes(_meshes);
						psr.mesh = _meshes[0];
					}

				ParticleStart();
			}

			if (m_Particles == null || m_Particles.Length < ps.main.maxParticles)
				m_Particles = new ParticleSystem.Particle[ps.main.maxParticles];
		}

		private ParticleSystem ps;
		private ParticleSystemRenderer psr;
		private ParticleSystem.Particle[] m_Particles;

		[SerializeField]
		private ParticleSystemRenderMode _renderMode;
		[SerializeField]
		private Material[] _materials;
		[SerializeField]
		[Tooltip("Remain null is ok if renderMode is not Mesh")]
		private Mesh[] _meshes;
	}
}
