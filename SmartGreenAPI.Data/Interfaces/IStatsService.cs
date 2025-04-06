using SmartGreenAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGreenAPI.Data.Interfaces
{
    public interface IStatsService
    {
        public Task<List<DailyStat>> SetDailyAVG();
    }
}
