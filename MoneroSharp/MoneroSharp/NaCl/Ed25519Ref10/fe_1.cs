using System;

namespace MoneroSharp.NaCl.Internal.Ed25519Ref10
{
	public static partial class FieldOperations
	{
		public static void fe_1(out FieldElement h)
		{
			h = default(FieldElement);
			h.x0 = 1;
		}
	}
}