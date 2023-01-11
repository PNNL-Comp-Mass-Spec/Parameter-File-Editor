using System.Collections;

namespace ParamFileGenerator
{
    public class EnzymeCollection : CollectionBase
    {

        public EnzymeCollection() : base()
        {
        }

        public void add(EnzymeDetails Enzyme)
        {
            List.Add(Enzyme);
        }

        public EnzymeDetails this[int index]
        {
            get
            {
                return (EnzymeDetails)List[index];
            }
            set
            {
                List[index] = value;
            }
        }
    }
}
