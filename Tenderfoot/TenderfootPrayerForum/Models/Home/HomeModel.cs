using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using TenderfootPrayerForum.Database;

namespace TenderfootPrayerForum.Models.Home
{
    public class HomeModel : TfModel
    {
        [Input]
        public int Count { get; set; } = 10;

        [JsonProperty]
        public List<dynamic> Posts { get; set; }

        public override IEnumerable<ValidationResult> Validate()
        {
            return null;
        }

        public override void MapModel()
        {
            var database = _DB.Posts;
            database.Case.OrderBy(database._(x => x.insert_time), Order.DESC);
            database.Case.LimitBy(this.Count);
            this.Posts = database.Select.Result;
        }
    }
}