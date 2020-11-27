using DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.Requests
{
    public class ExerciseRequest
    {
        public ExerciseType ExerciseType { get; set; }

        public int Score { get; set; }

        public string Description { get; set; }

        public byte[] Data { get; set; }

        public string Keywords { get; set; }

        public uint Order { get; set; }

        public int SyllabusId { get; set; }
    }
}
