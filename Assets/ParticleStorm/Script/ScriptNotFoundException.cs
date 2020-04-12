using System.Collections.Generic;

namespace ParticleStorm.Script
{
	class ScriptNotFoundException : KeyNotFoundException
	{
		public override string Message => "No " + type + " named " + script + ".";

		private readonly string script;
		private readonly string type;

		public ScriptNotFoundException(string script, string type)
		{
			this.script = script;
			this.type = type;
		}
	}
}
