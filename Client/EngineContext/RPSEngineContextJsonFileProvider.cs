using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Regdata.RPS.Engine.Client.EngineContext
{
    public class RPSEngineContextJsonFileProvider : RPSEngineContextProviderBase
    {
        private readonly string _rightsContextsFilePath;
        private readonly string _processingContextsFilePath;

        public RPSEngineContextJsonFileProvider(string rightsContextsFilePath, string processingContextsFilePath)
        {
            _rightsContextsFilePath = rightsContextsFilePath;
            _processingContextsFilePath = processingContextsFilePath;
        }

        protected override IReadOnlyDictionary<string, RightsContext> GetRightsContexts()
            => GetDictionaryFromJsonFile<string, RightsContext>(_rightsContextsFilePath);

        protected override IReadOnlyDictionary<string, ProcessingContext> GetProcessingContexts()
            => GetDictionaryFromJsonFile<string, ProcessingContext>(_processingContextsFilePath);

        private static IReadOnlyDictionary<TKey, TValue> GetDictionaryFromJsonFile<TKey, TValue>(string filePath)
            => JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(File.ReadAllText(filePath));
    }
}
