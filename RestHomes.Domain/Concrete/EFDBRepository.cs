using RestHomes.Domain.Abstract;
using RestHomes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestHomes.Domain.Concrete
{
    public class EFDBRepository : IDBRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<Worker> Workers
        {
            get { return context.Workers; }
        }
        public IQueryable<Shift> Shifts
        {
            get { return context.Shifts; }
        }
        public IQueryable<Position> Positions
        {
            get { return context.Positions; }
        }
        public IQueryable<TimeSheet> TimeSheets
        {
            get { return context.TimeSheets; }
        }
        public void AddPositionsToShift(int shiftId, int[] posIds)
        {
            Shift shift = context.Shifts.Find(shiftId);
            if (shift != null)
            {
                foreach (int posId in posIds ?? new int[] { })
                {
                    Position pos = context.Positions.Find(posId);
                    if (pos != null)
                    {
                        shift.Positions.Add(pos);
                        context.SaveChanges();
                    }
                }
            }
        }
        public void RemPositionsFromShift(int shiftId, int[] posIds)
        {
            Shift shift = context.Shifts.Find(shiftId);
            if (shift != null)
            {
                foreach (int posId in posIds ?? new int[] { })
                {
                    Position pos = context.Positions.Find(posId);
                    if (pos != null)
                    {
                        shift.Positions.Remove(pos);
                        context.SaveChanges();
                    }
                }
            }
        }
        public void AddPositionsToWorker(int workerId, int[] posIds)
        {
            Worker worker = context.Workers.Find(workerId);
            if (worker != null)
            {
                foreach (int posId in posIds ?? new int[] { })
                {
                    Position pos = context.Positions.Find(posId);
                    if (pos != null)
                    {
                        worker.Positions.Add(pos);
                        context.SaveChanges();
                    }
                }
            }
        }
        public void RemPositionsFromWorker(int workerId, int[] posIds)
        {
            Worker worker = context.Workers.Find(workerId);
            if (worker != null)
            {
                foreach (int posId in posIds ?? new int[] { })
                {
                    Position pos = context.Positions.Find(posId);
                    if (pos != null)
                    {
                        worker.Positions.Remove(pos);
                        context.SaveChanges();
                    }
                }
            }
        }
        public void AddWorkersToPosition(int posId, int[] workerIds)
        {
            Position pos = context.Positions.Find(posId);

            foreach (int workerId in workerIds ?? new int[] { })
            {
                Worker worker = context.Workers.Find(workerId);
                if (worker != null)
                {
                    pos.Workers.Add(worker);
                    context.SaveChanges();
                }
            }
        }
        public void RemWorkersFromPosition(int posId, int[] workerIds)
        {
            Position pos = context.Positions.Find(posId);

            foreach (int workerId in workerIds ?? new int[] { })
            {
                Worker worker = context.Workers.Find(workerId);
                if (worker != null)
                {
                    pos.Workers.Remove(worker);
                    context.SaveChanges();
                }
            }
        }
        public void AddShiftsToPosition(int posId, int[] shiftIds)
        {
            Position pos = context.Positions.Find(posId);

            foreach (int shId in shiftIds ?? new int[] { })
            {
                Shift shift = context.Shifts.Find(shId);
                if (shift != null)
                {
                    pos.Shifts.Add(shift);
                    context.SaveChanges();
                }
            }
        }
        public void RemShiftsFromPosition(int posId, int[] shiftIds)
        {
            Position pos = context.Positions.Find(posId);

            foreach (int shId in shiftIds ?? new int[] { })
            {
                Shift shift = context.Shifts.Find(shId);
                if (shift != null)
                {
                    pos.Shifts.Remove(shift);
                    context.SaveChanges();
                }
            }
        }

        public int DeletePosition(int positionId)
        {
            Position pos = context.Positions.Find(positionId);
            if (pos != null)
            {
                IEnumerable<TimeSheet> tsSig = context.TimeSheets.Where(ts => (ts.isSigned == true) && ts.IDp == pos.IDp);
                IEnumerable<TimeSheet> tsUnSig = context.TimeSheets.Where(ts => (ts.isSigned != true) && ts.IDp == pos.IDp);
                context.TimeSheets.RemoveRange(tsUnSig);

                foreach (var worker in pos.Workers)
                {
                    worker.Positions.Remove(pos);
                }
 
                foreach (var shift in pos.Shifts)
                {
                    shift.Positions.Remove(pos);
                }

                if (tsSig.Count() > 0)
                {
                    pos.isDeleted = true;
                    pos.Shifts.Clear();
                }
                else
                {
                    context.Positions.Remove(pos);
                }
                //context.Shifts
                
                context.SaveChanges();
            }
            return 0;
        }

        public int DeleteWorker(int workerId)
        {
            return 0;
        }

        public int DeleteShift(int shiftId)
        {
            Shift sh = context.Shifts.Find(shiftId);
            if (sh != null)
            {
                IEnumerable<TimeSheet> tsSig = context.TimeSheets.Where(ts => (ts.isSigned == true) && ts.IDs == sh.IDs);
                IEnumerable<TimeSheet> tsUnSig = context.TimeSheets.Where(ts => (ts.isSigned != true) && ts.IDs == sh.IDs);
                context.TimeSheets.RemoveRange(tsUnSig);

                foreach (var pos in sh.Positions)
                {
                    pos.Shifts.Remove(sh);
                }

                if (tsSig.Count() > 0)
                {
                    sh.isDeleted = true;
                    sh.Positions.Clear();
                }
                else
                {
                    context.Shifts.Remove(sh);
                }

                context.SaveChanges();
            }
            return 0;

        }

        public Worker GetWorker(int id)
        {
            return context.Workers.Find(id);
        }
        public Shift GetShift(int id)
        {
            return context.Shifts.Find(id);
        }
        public Position GetPosition(int id)
        {
            return context.Positions.Find(id);
        }
        public int SavePosition(Position pos)
        {
            int ret = 0;
            IEnumerable<Position> allPositions = context.Positions.Where(p => p.Name.Equals(pos.Name));
            if (allPositions.Count() == 0)
            {
                context.Positions.Add(pos);
                ret = context.SaveChanges();
            }
            else
            {
                if (allPositions.Count() == 1 && allPositions.ElementAt(0).isDeleted==true)
                {
                    Position role = allPositions.ElementAt(0);
                    role.isDeleted = false;
                    IEnumerable<Shift> shs = context.TimeSheets.Where(ts => ts.IDp == role.IDp).Select(ts => ts.Shift);
                    foreach (var sh in shs)
                    {
                        sh.Positions.Add(allPositions.ElementAt(0));
                        allPositions.ElementAt(0).Shifts.Add(sh);
                    }
                    ret = context.SaveChanges();
                }
            }
            return ret;
        }
        public int SaveShift(Shift shift)
        {
            int ret = 0;
            context.Shifts.Add(shift);
            ret = context.SaveChanges();
            return ret;
        }
        public int SaveWorker(Worker worker)
        {
            int ret = 0;
            context.Workers.Add(worker);
            ret = context.SaveChanges();
            return ret;
        }
        public void SaveToTS(int posId, int shiftId, IList<List<int>> fortNight, DateTime dt)
        {
            DateTime firstDay = dt;
            DateTime lastDay  = dt.AddDays(13);
            
            IEnumerable<TimeSheet> ts = (from times in TimeSheets
                                           where times.Day <= lastDay && times.Day >= firstDay && times.IDs == shiftId && times.IDp == posId
                                           select times).ToArray();
                                           //.TakeWhile(t => (t.Day >= dt) && (t.Day <= dt.AddDays(13)) && (shiftId == t.IDs)).ToArray();
            // if ts.Count() > 1 Throw TODO
            for (int i = 0; i < fortNight.Count(); i++)
            {
                List<int> workerIDs = fortNight.ElementAt(i);
                DateTime dayi = dt.AddDays(i);
                IEnumerable<TimeSheet> tsi = ts.Where(t => (t.Day == dayi)&&(t.isSigned!=true));
                
                if (workerIDs != null)
                {
                    Shift shiftToAdd = context.Shifts.Find(shiftId);
                    IEnumerable<TimeSheet> toAdd = workerIDs.Where(wID => !tsi.Any(t2 => t2.IDw == wID))
                        .Select(id => new TimeSheet { IDw = id, IDs = shiftId, Day = dayi, IDp = posId, Hours=shiftToAdd.Hours });
                    IEnumerable<TimeSheet> toDel = tsi.Where(t => !workerIDs.Any(t2 => t2 == t.IDw));
                    context.TimeSheets.AddRange(toAdd);
                    if (context.SaveChanges() != toAdd.Count()) { /*Something goes wrong*/ }
                    context.TimeSheets.RemoveRange(toDel);
                    if (context.SaveChanges() != toDel.Count()) { /*Something goes wrong*/ }
                }
                else
                {
                    context.TimeSheets.RemoveRange(tsi);
                    context.SaveChanges();
                }
            }

                return;
        }
    }
}
