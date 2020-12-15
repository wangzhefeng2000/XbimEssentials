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
namespace Xbim.Ifc4.ApprovalResource
{
	public partial class IfcApproval : IExpressValidatable
	{
		public enum IfcApprovalClause
		{
			HasIdentifierOrName,
		}

		/// <summary>
		/// Tests the express where-clause specified in param 'clause'
		/// </summary>
		/// <param name="clause">The express clause to test</param>
		/// <returns>true if the clause is satisfied.</returns>
		public bool ValidateClause(IfcApprovalClause clause) {
			var retVal = false;
			try
			{
				switch (clause)
				{
					case IfcApprovalClause.HasIdentifierOrName:
						retVal = Functions.EXISTS(Identifier) || Functions.EXISTS(Name);
						break;
				}
			} catch (Exception  ex) {
				var log = Validation.ValidationLogging.CreateLogger<Xbim.Ifc4.ApprovalResource.IfcApproval>();
				log?.LogError(string.Format("Exception thrown evaluating where-clause 'IfcApproval.{0}' for #{1}.", clause,EntityLabel), ex);
			}
			return retVal;
		}

		public virtual IEnumerable<ValidationResult> Validate()
		{
			if (!ValidateClause(IfcApprovalClause.HasIdentifierOrName))
				yield return new ValidationResult() { Item = this, IssueSource = "IfcApproval.HasIdentifierOrName", IssueType = ValidationFlags.EntityWhereClauses };
		}
	}
}
