using RestHomes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RestHomes.Models
{

    public class RosterWorkersByDaysForShiftId
    {
        public DateTime dtFirst {get;set;}
        public IList<WorkersByDaysForShiftId> wbdfs { get; set; }
    }
    public class WorkersByDaysForShiftId
    {
        public int shiftId { get; set; }
        public int posId { get; set; }
        public List<int> day_0 { get; set; }
        public List<int> day_1 { get; set; }
        public List<int> day_2 { get; set; }
        public List<int> day_3 { get; set; }
        public List<int> day_4 { get; set; }
        public List<int> day_5 { get; set; }
        public List<int> day_6 { get; set; }

        public List<int> day_7 { get; set; }
        public List<int> day_8 { get; set; }
        public List<int> day_9 { get; set; }
        public List<int> day_10 { get; set; }
        public List<int> day_11 { get; set; }
        public List<int> day_12 { get; set; }
        public List<int> day_13 { get; set; }

    }
    public class Shift2Days
    {
        public Shift shift { get; set; }
        public IEnumerable<IEnumerable<Worker>> worker2days { get; set; }
    }
    public class Position2Shifts
    {
        public Position position { get; set; }
        public IEnumerable<Shift2Days> shifts { get; set; }
        //public IEnumerable<Worker> workers { get; set; }
    }
    public class TimeSheetModel
    {
        public IEnumerable<Workers2HrsGr> workers { get; set; }
        public IEnumerable<DateTime> days { get; set; }
    }

    public class HrsKey
    {
        public string key { get; set; }
        public double hrs { get; set; }
    }
    public class Workers2Hrs
    {
        public Worker worker { get; set; }
        public IEnumerable<IEnumerable<HrsKey>> hours2days { get; set; }
    }
    public class Workers2HrsGr
    {
        public Worker worker { get; set; }
        public IEnumerable<IEnumerable<IGrouping<string, HrsKey>>> hours2days { get; set; }
    }

    public class RosterModel
    {
        //public IQueryable<Position2Shifts> positions { get; set; }
        public IEnumerable<Position2Shifts> positions { get; set; }
        public IEnumerable<DateTime> days { get; set; }
    }
    public class PositionModel
    {
        public IEnumerable<Position2Names> pos2names { get; set; }
    }
    public class Position2Names
    {
        public Worker worker { get; set; }
        public bool isHisPosition { get; set; }
    }
}