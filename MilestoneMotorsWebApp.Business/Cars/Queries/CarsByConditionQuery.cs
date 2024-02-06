using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Domain.Enums;

namespace MilestoneMotorsWebApp.Business.Cars.Queries
{
    public class CarsByCondition
    {
        public static List<Car> ApplyConditionFilter(List<Car> source, string condition)
        {
            if (!string.IsNullOrEmpty(condition))
            {
                var selectedcondition = Enum.Parse<Condition>(condition);
                source = source.Where(c => c.Condition == selectedcondition).ToList();
            }
            return source;
        }
    }
}
