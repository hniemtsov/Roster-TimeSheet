using RestHomes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RestHomes.Models
{
    public class Workers2Hours
    {
        public Worker worker { get; set; }
        public bool signed { get; set; }
        //public IEnumerable<double> hours2days { get; set; }
        public IEnumerable<double> hours2days { get; set; }
    }

    public class CreateWorkerModel
    {
        [Required]
        [StringLength(12, MinimumLength = 3)]
        public string firstName { get; set; }
        [Required]
        [StringLength(12, MinimumLength = 3)]
        public string lastName { get; set; }
        [Required]
        [StringLength(5, MinimumLength=2)]
        public string nickName { get; set; }
        [Required]
        [Range(0, 100)]
        public int sickHours { get; set; }
        [Required]
        [Range(0, 100)]
        public int leaveHours { get; set; }
    }


    public class CreateShiftModel
    {
        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = @"{0:hh\:mm}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"((([0-1][0-9])|(2[0-3]))(:[0-5][0-9])(:[0-5][0-9])?)", ErrorMessage = "Time must be between 00:00 to 23:59")]
        public TimeSpan Start { get; set; }
        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = @"{0:hh\:mm}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"((([0-1][0-9])|(2[0-3]))(:[0-5][0-9])(:[0-5][0-9])?)", ErrorMessage = "Time must be between 00:00 to 23:59")]
        public TimeSpan End { get; set; }
        [Required]
        [Range(0.5, 24.0)]
        public double Hours { get; set; }
    }


    public class CreatePositionModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }

    public class PositionShiftsEditModel
    {
        public Position Pos { get; set; }
        public IEnumerable<Shift> Members { get; set; }
        public IEnumerable<Shift> NonMembers { get; set; }
        public string ReturnUrl;
    }
    public class PositionEditModel
    {
        public Position Pos { get; set; }
        public IEnumerable<Worker> Members { get; set; }
        public IEnumerable<Worker> NonMembers { get; set; }
        public string ReturnUrl;
    }

    public class WorkerEditModel
    {
        public Worker worker { get; set; }
        public IEnumerable<Position> Members { get; set; }
        public IEnumerable<Position> NonMembers { get; set; }
        public string ReturnUrl;
    }

    public class ShiftEditModel
    {
        public Shift shift { get; set; }
        public IEnumerable<Position> Members { get; set; }
        public IEnumerable<Position> NonMembers { get; set; }
        public string ReturnUrl;
    }
    public class ManyModificationModel
    {
        [Required]
        public int modId { get; set; }
        public int[] IdsToAdd { get; set; }
        public int[] IdsToDelete { get; set; }
    }
}