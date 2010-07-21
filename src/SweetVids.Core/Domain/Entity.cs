

using System;

namespace SweetVids.Core.Domain
{
    [Serializable]
    public abstract class Entity : IEquatable<Entity>
    {
        public static int UnboundedStringLength = 2000;
        public virtual Guid Id { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual DateTime LastUpdated { get; set; }

        protected Entity()
        {
            Created = DateTime.Now;
            LastUpdated = DateTime.Now;
        }

        #region IEquatable<Entity> Members

        /// <summary>
        /// Indicates whether the current <see cref="T:SweetVids.Core.Domain.Entity" /> is equal to another <see cref="T:SweetVids.Core.Domain.Entity" />.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">A Entity to compare with this object.</param>
        public virtual bool Equals(Entity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            var otherIsTransient = Equals(other.Id, default(Guid));
            var thisIsTransient = Equals(Id, default(Guid));

            if (otherIsTransient && thisIsTransient)
                return ReferenceEquals(this, other);

            return other.Id.Equals(Id);
        }

        #endregion

        /// <summary>
        /// Determines whether the specified <see cref="T:SweetVids.Core.Domain.Entity" /> is equal to the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:SweetVids.Core.Domain.Entity" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />. </param>
        /// <exception cref="T:System.NullReferenceException">The <paramref name="obj" /> parameter is null.</exception><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            //if (ReferenceEquals(null, obj)) return false;
            //if (ReferenceEquals(this, obj)) return true;
            //if (obj.GetType() != GetType()) return false;
            //return Equals((Entity) obj);

            var other = obj as Entity;

            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            var otherIsTransient = Equals(other.Id, default(Guid));
            var thisIsTransient = Equals(Id, default(Guid));

            if (otherIsTransient && thisIsTransient)
                return ReferenceEquals(this, other);

            var isIdEqual = other.Id.Equals(Id);

            if (isIdEqual)
            {
                Type objType = obj.GetType();
                Type myType = GetType();

                if (objType.Name.Contains(myType.Name) && objType.Name.Contains("Proxy"))
                    return true;

                return obj.GetType() == GetType();
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for a Entity. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return Equals(Id, default(Guid))
                       ? base.GetHashCode()
                       : Id.GetHashCode();
        }

        public static bool operator ==(Entity left, Entity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !Equals(left, right);
        }
    }
}