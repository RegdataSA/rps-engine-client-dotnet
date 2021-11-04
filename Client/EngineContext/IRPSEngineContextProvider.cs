namespace Regdata.RPS.Engine.Client.EngineContext
{
    public interface IRPSEngineContextProvider
    {
        void Initialize();

        bool TryGetRightsContext(string contextKey, out RightsContext rightsContext);

        bool TryGetProcessingContext(string contextKey, out ProcessingContext processingContext);
    }
}
