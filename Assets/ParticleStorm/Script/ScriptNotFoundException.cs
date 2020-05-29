using System;
using System.Collections.Generic;

namespace ParticleStorm.Script
{
	class ScriptNotFoundException : KeyNotFoundException
	{
		public override string Message
		{
			get
			{
				if (message == null)
					return "No " + type + " named " + script + ".";
				else
					return message;
			}
		}

		private readonly string script;
		private readonly string type;
		private readonly string message;

		public ScriptNotFoundException(string script, string type)
		{
			this.script = script;
			this.type = type;
		}

		public ScriptNotFoundException()
		{
			message = "";
		}

		public ScriptNotFoundException(string message) : base(message)
		{
			this.message = message;
		}

		public ScriptNotFoundException(string message, System.Exception innerException) : base(message, innerException)
		{
			this.message = message;
		}
	}
}
