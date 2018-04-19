using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Database;
using Tenderfoot.Database.Tables;
using Tenderfoot.TfSystem;

namespace Tenderfoot.Mvc
{
    public class AuthorizeModel : TfModel
    {
        public Schema<Accesses> Accesses { get; set; } = Schemas.Accesses;

        public override void BeforeStartUp()
        {
            this.Accesses.Conditions.Where(this.Accesses.Column(x => x.key), Is.EqualTo, this.Key);
            this.Accesses.Conditions.Where(this.Accesses.Column(x => x.active), Is.EqualTo, 1);
            this.Accesses.Conditions.LimitBy(1);
        }

        [Input]
        [ValidateInput(InputType.String, 50)]
        public string Key { get; set; }
        
        [JsonProperty]
        public string Secret { get; set; }

        public override void HandleModel()
        {
            throw new NotImplementedException();
        }

        public override void MapModel()
        {
            var entity = this.Accesses.Select.Entity;
            this.Secret = Encryption.Encrypt(entity.key + entity.secret + entity.host);
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            yield return TfValidationResult.FieldRequired(nameof(this.Key), this.Key);
            if (this.Accesses.Count() == 0)
            {
                yield return TfValidationResult.Compose("Unauthorized");
            }
        }
    }
}
