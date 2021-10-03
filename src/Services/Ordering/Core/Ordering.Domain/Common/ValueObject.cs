using System;
using System.Collections.Generic;
using System.Linq;

namespace Ordering.Domain.Common
{
	//https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/implement-value-objects	
	public abstract class ValueObject
	{
		//Implementation Example
		//protected override IEnumerable<object> GetEqualityComponents()
		//{
		//	//Using a yield return statement to return each element one at a time
		//	yield return x;
		//	yield return y;
		//	yield return Z;
		//  if you have an array type
		//	foreach (array item in items)
		//  {
		//   yield return item;
		//  }
		//}

		protected abstract IEnumerable<object> GetEqualityComponents();

		protected static bool EqualOperator(ValueObject Left, ValueObject Right)
		{
			//Old snippet implementation
			//bool leftValueCheck = ReferenceEquals(Left, null);
			//if (leftValueCheck ^ ReferenceEquals(Right, null))
			//{
			//	return false;
			//}
			//return leftValueCheck || Left.Equals(Right);

			if (Left is null ^ Right is null)
			{
				return false;
			}

			return Left?.Equals(Right) != false;
		}

		protected static bool NotEqualOperator(ValueObject Left, ValueObject Right)
		{
			return !EqualOperator(Left, Right);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != GetType())
			{
				return false;
			}

			ValueObject other = (ValueObject)obj;
			return this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
		}

		public override int GetHashCode()
		{
			return GetEqualityComponents().Select(x => x != null ? x.GetHashCode() : 0).Aggregate((X, Y) => X ^ Y);
		}
	}
}
