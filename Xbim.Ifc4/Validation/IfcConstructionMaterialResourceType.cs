using System;
using Microsoft.Extensions.Logging;
using Xbim.Common;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Xbim.Common.Enumerations;
using Xbim.Common.ExpressValidation;
using Xbim.Ifc4.Interfaces;
// ReSharper disable once CheckNamespace
// ReSharper disable InconsistentNaming
namespace Xbim.Ifc4.ConstructionMgmtDomain
{
	public partial class IfcConstructionMaterialResourceType : IExpressValidatable
	{
		public enum IfcConstructionMaterialResourceTypeClause
		{
			CorrectPredefinedType,
		}

		/// <summary>
		/// Tests the express where-clause specified in param 'clause'
		/// </summary>
		/// <param name="clause">The express clause to test</param>
		/// <returns>true if the clause is satisfied.</returns>
		public bool ValidateClause(IfcConstructionMaterialResourceTypeClause clause) {
			var retVal = false;
			try
			{
				switch (clause)
				{
					case IfcConstructionMaterialResourceTypeClause.CorrectPredefinedType:
						retVal = (PredefinedType != IfcConstructionMaterialResourceTypeEnum.USERDEFINED) || ((PredefinedType == IfcConstructionMaterialResourceTypeEnum.USERDEFINED) && Functions.EXISTS(this/* as IfcTypeResource*/.ResourceType));
						break;
				}
			} catch (Exception  ex) {
				var log = Validation.ValidationLogging.CreateLogger<Xbim.Ifc4.ConstructionMgmtDomain.IfcConstructionMaterialResourceType>();
				log?.LogError(string.Format("Exception thrown evaluating where-clause 'IfcConstructionMaterialResourceType.{0}' for #{1}.", clause,EntityLabel), ex);
			}
			return retVal;
		}

		public override IEnumerable<ValidationResult> Validate()
		{
			foreach (var value in base.Validate())
			{
				yield return value;
			}
			if (!ValidateClause(IfcConstructionMaterialResourceTypeClause.CorrectPredefinedType))
				yield return new ValidationResult() { Item = this, IssueSource = "IfcConstructionMaterialResourceType.CorrectPredefinedType", IssueType = ValidationFlags.EntityWhereClauses };
		}
	}
}
