using EMS.Core.Configuration;

namespace EMS.Core.Requests.ServiceAppointments;

public class GetAllServiceAppointmentsRequest : PagedRequest
{
    public GetAllServiceAppointmentsRequest() { }

    public GetAllServiceAppointmentsRequest(int pageSize = ConfigurationDefault.DefaultPageSize, int pageNumber = ConfigurationDefault.DefaultPageNumber, string query = null) : base(pageSize, pageNumber, query)
    {

    }
}
