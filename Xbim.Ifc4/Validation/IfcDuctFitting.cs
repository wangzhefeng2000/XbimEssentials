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
namespace Xbim.Ifc4.HvacDomain
{
	public partial class IfcDuctFitting : IExpressValidatable
	{
		public enum IfcDuctFittingClause
		{
			CorrectPredefinedType,
			CorrectTypeAssigned,
		}

		/// <summary>
		/// Tests the express where-clause specified in param 'clause'
		/// </summary>
		/// <param name="clause">The express clause to test</param>
		/// <returns>true if the clause is satisfied.</returns>
		public bool ValidateClause(IfcDuctFittingClause clause) {
			var retVal = false;
			try
			{
				switch (clause)
				{
					case IfcDuctFittingClause.CorrectPredefinedType:
						retVal = !(Functions.EXISTS(PredefinedType)) || (PredefinedType != IfcDuctFittingTypeEnum.USERDEFINED) || ((PredefinedType == IfcDuctFittingTypeEnum.USERDEFINED) && Functions.EXISTS(this/* as IfcObject*/.ObjectType));
						break;
					case IfcDuctFittingClause.CorrectTypeAssigned:
						retVal = (Functions.SIZEOF(IsTypedBy) == 0) || (Functions.TYPEOF(this/* as IfcObject*/.IsTypedBy.ItemAt(0).RelatingType).Contains("IFC4.IFCDUCTFITTINGTYPE"));
						break;
				}
			} catch (Exception  ex) {
				var log = Validation.ValidationLogging.CreateLogger<Xbim.Ifc4.HvacDomain.IfcDuctFitting>();
				log?.LogError(string.Format("Exception thrown evaluating where-clause 'IfcDuctFitting.{0}' for #{1}.", clause,EntityLabel), ex);
			}
			return retVal;
		}

		public override IEnumerable<ValidationResult> Validate()
		{
			foreach (var value in base.Validate())
			{
				yield return value;
			}
			if (!ValidateClause(IfcDuctFittingClause.CorrectPredefinedType))
				yield return new ValidationResult() { Item = this, IssueSource = "IfcDuctFitting.CorrectPredefinedType", IssueType = ValidationFlags.EntityWhereClauses };
			if (!ValidateClause(IfcDuctFittingClause.CorrectTypeAssigned))
				yield return new ValidationResult() { Item = this, IssueSource = "IfcDuctFitting.CorrectTypeAssigned", IssueType = ValidationFlags.EntityWhereClauses };
		}
	}
}
