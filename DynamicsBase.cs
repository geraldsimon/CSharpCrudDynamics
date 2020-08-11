namespace CSharpCrudDynamics
{
    public abstract class DynamicsBase 
    {
        protected readonly DynamicsContext Context;
        public DynamicsBase()
        {
            Context = DynamicsContext.GetInstance();
        }
    }
}