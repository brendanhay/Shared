namespace Infrastructure
{
    public static class InfrastructureBootstrapper
    {
        public static void Setup(IServiceLocator locator)
        {
            locator.Add<ICryptoService, CryptoService>(false);
            locator.Add<ICompilerService, CompilerService>(false);
        }
    }
}
