using RestHomes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestHomes.Domain.Abstract
{
    public interface IDBRepository
    {
        IQueryable<Worker> Workers { get; }
        IQueryable<Shift> Shifts { get; }
        IQueryable<Position> Positions { get; }
        IQueryable<TimeSheet> TimeSheets { get; }
        void SaveToTS(int posId, int shiftId, IList<List<int>> fortNight, DateTime dt);
        
        void AddWorkersToPosition(int posId, int[] workerId);
        void RemWorkersFromPosition(int posId, int[] workerId);
       
        void AddShiftsToPosition(int posId, int[] shiftId);
        void RemShiftsFromPosition(int posId, int[] shiftId);

        void AddPositionsToWorker(int workerId, int[] posIds);
        void RemPositionsFromWorker(int workerId, int[] posIds);

        void AddPositionsToShift(int shiftId, int[] posIds);
        void RemPositionsFromShift(int workerId, int[] posIds);
  
        int SavePosition(Position pos);
        int SaveShift(Shift pos);
        int SaveWorker(Worker pos);

        Position GetPosition(int id);
        Shift GetShift(int id);
        Worker GetWorker(int id);

        int DeletePosition(int positionId);
        int DeleteShift(int shiftId);
        int DeleteWorker(int workerId);
    }
}
