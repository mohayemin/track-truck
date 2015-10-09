﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Ssi.TrackTruck.Bussiness.DAL.Constants;
using Ssi.TrackTruck.Bussiness.DAL.Trips;
using Ssi.TrackTruck.Bussiness.Helpers;

namespace Ssi.TrackTruck.Bussiness.Trips
{
    public class TripDropRequest : IValidatableObject
    {
        [Required(ErrorMessage = "Please choose branch")]
        public string BranchId { get; set; }
        public DateTimeModel ExpectedDropTime { get; set; }
        public IList<DeliveryReceiptRequest> DeliveryReceipts { get; set; }

        public DbDrop ToDrop()
        {
            return new DbDrop
            {
                BranchId = BranchId,
                ExpectedDropTime = ExpectedDropTime.ToDateTime(DateTimeConstants.PhilippineOffset),
                DeliveryReceipts = DeliveryReceipts.Select(dr => dr.ToDeliveryReceipt()).ToList()
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ExpectedDropTime == null || ExpectedDropTime.IsEmpty)
            {
                yield return new ValidationResult("Please choose expected drop time");
            }
        }
    }
}