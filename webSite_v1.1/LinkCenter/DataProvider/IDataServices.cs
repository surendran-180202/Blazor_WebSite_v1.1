using LinkCenter.DataModal;

namespace LinkCenter.DataProvider
{
    public interface IDataServices
    {
        List<tblExperience> Experience();
        List<tblEdnModal> Education();
    }
}
