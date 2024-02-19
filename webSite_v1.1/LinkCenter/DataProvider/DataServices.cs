using System.Data.SqlClient;
using System.Text.Json;

using LinkCenter.DataModal;

namespace LinkCenter.DataProvider
{
    public class DataServices  /*: IDataServices*/
    {
        DataAccessLayer dataAccessLayer = new DataAccessLayer();
        public List<tblExperience> Experience()
        {
            List<tblExperience> Experience = dataAccessLayer.GetAllExperience().ToList();
            return Experience;
        }

        public List<tblEdnModal> Education()
        {
            List<tblEdnModal> Education = dataAccessLayer.GetAllEducation().ToList();
            return Education;
        }

    }
}
