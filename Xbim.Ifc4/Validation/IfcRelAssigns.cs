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
namespace Xbim.Ifc4.Kernel
{
	public partial class IfcRelAssigns : IExpressValidatable
	{
		public enum IfcRelAssignsClause
		{
			WR1,
		}

		/// <summary>
		/// Tests the express where-clause specified in param 'clause'
		/// </summary>
		/// <param name="clause">The express clause to test</param>
		/// <returns>true if the clause is satisfied.</returns>
		public bool ValidateClause(IfcRelAssignsClause clause) {
			var retVal = false;
			try
			{
				switch (clause)
				{
					case IfcRelAssignsClause.WR1:
						retVal = Functions.IfcCorrectObjectAssignment(RelatedObjectsType, RelatedObjects);
						break;
				}
			} catch (Exception  ex) {
				var log = Validation.ValidationLogging.CreateLogger<Xbim.Ifc4.Kernel.IfcRelAssigns>();
				log?.LogError(string.Format("Exception thrown evaluating where-clause 'IfcRelAssigns.{0}' for #{1}.", clause,EntityLabel), ex);
			}
			return retVal;
		}

		public virtual IEnumerable<ValidationResult> Validate()
		{
			if (!ValidateClause(IfcRelAssignsClause.WR1))
				yield return new ValidationResult() { Item = this, IssueSource = "IfcRelAssigns.WR1", IssueType = ValidationFlags.EntityWhereClauses };
		}
	}
}
