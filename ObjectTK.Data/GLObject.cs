namespace ObjectTK.Data {
	public abstract class GLObject {

		public int Handle { get; }

		private GLObject() {

		}

		public GLObject(int Handle) {
			this.Handle = Handle;
		}

        public bool Equals(GLObject Other) {
            return Other != null && Handle.Equals(Other.Handle);
        }

        public override bool Equals(object Other) {
            return Other is GLObject GLObject && Equals(GLObject);
        }

        public override int GetHashCode() {
            return Handle.GetHashCode();
        }

    }
}
