using System.Runtime.Serialization;

namespace MemoryGame.Contracts
{
    [DataContract]
    public class SelectedCard
    {
        [DataMember]
        public int Row { get; set; }

        [DataMember]
        public int Column { get; set; }

        [DataMember]
        public int ResourceIndex { get; set; }

        [DataMember]
        public string ResourceSet { get; set; }
    }
}