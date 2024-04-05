using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Core.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UniqueAttribute : Attribute
    {
        internal static void CheckAttribute(ModelBuilder builder, Type entityType, PropertyInfo[] columns)
        {
            foreach (var it in columns)
            {
                if (it.GetCustomAttribute<UniqueAttribute>() == null)
                    continue;

                builder.Entity(entityType)
                     .HasIndex(it.Name)
                     .IsUnique();
            }
        }
    }
}
