//
//          Copyright Seth Hendrick 2019.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.Linq;
using System.Reflection;

[AttributeUsage( AttributeTargets.Property, Inherited = true, AllowMultiple = false )]
public class StringArgumentAttribute : Attribute
{
    // ---------------- Constructor ----------------

    public StringArgumentAttribute( string arg )
    {
        if( string.IsNullOrWhiteSpace( arg ) )
        {
            throw new ArgumentNullException( nameof( arg ) );
        }
        this.ArgName = arg;
        this.DefaultValue = string.Empty;
        this.Description = string.Empty;
        this.Required = false;
    }

    // ---------------- Properties ----------------

    /// <summary>
    /// The default value of the argument; defaulted to <see cref="string.Empty"/>.
    /// </summary>
    public string DefaultValue { get; set; }

    /// <summary>
    /// The argument name 
    /// </summary>
    public string ArgName { get; private set; }

    /// <summary>
    /// Description of what the argument does.
    /// </summary>
    public string Description{ get; set; }

    /// <summary>
    /// If the value is an empty string, this will fail validation.
    /// </summary>
    public bool Required{ get; set; }

    // ---------------- Functions ----------------

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine( "\t- " + this.ArgName );
        builder.AppendLine( "\t\t" + this.Description );
        if( string.IsNullOrWhiteSpace( this.DefaultValue ) == false )
        {
            builder.AppendLine( "\t\tDefaulted to: " + this.DefaultValue );
        }
        else
        {
            builder.AppendLine( "\t\tDefaulted to: (Empty String)" );
        }
        builder.AppendLine( "\t\tRequired: " + this.Required );

        return builder.ToString();
    }

    /// <summary>
    /// Validates this object.  Returns <see cref="string.Empty"/>
    /// if nothing is wrong, otherwise this returns an error message.
    /// </summary>
    public string TryValidate()
    {
        StringBuilder builder = new StringBuilder();
        if( string.IsNullOrWhiteSpace( this.ArgName ) )
        {
            builder.AppendLine( nameof( this.ArgName ) + " can not be null, empty, or whitespace." );
        }
        if( string.IsNullOrWhiteSpace( this.Description ) )
        {
            builder.AppendLine( nameof( this.Description ) + " can not be null, empty, or whitespace." );
        }

        return builder.ToString();
    }
}

public static class ArgumentHelpers
{
    public static string GetDescription<T>( string taskDescription )
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine( taskDescription );
        bool addedArgumentString = false;

        Type type = typeof(T);
        IEnumerable<PropertyInfo> properties = type.GetProperties();
        foreach( PropertyInfo info in properties )
        {
            StringArgumentAttribute argumentAttribute = info.GetCustomAttribute<StringArgumentAttribute>();
            if( argumentAttribute != null )
            {
                if( addedArgumentString == false )
                {
                    builder.AppendLine( "- Arguments:" );
                    addedArgumentString = true;
                }
                builder.AppendLine( argumentAttribute.ToString() );
            }
        }

        return builder.ToString();
    }

    public static T FromArguments<T>( ICakeContext cakeContext, params object[] constructorArgs )
    {
        Type type = typeof(T);
        T instance = (T)Activator.CreateInstance( type, constructorArgs );

        StringBuilder errors = new StringBuilder();

        IEnumerable<PropertyInfo> properties = type.GetProperties();
        foreach( PropertyInfo info in properties )
        {
            StringArgumentAttribute argumentAttribute = info.GetCustomAttribute<StringArgumentAttribute>();
            if( argumentAttribute != null )
            {
                string argumentErrors = argumentAttribute.TryValidate();
                if( string.IsNullOrWhiteSpace( argumentErrors ) == false )
                {
                    errors.AppendLine( argumentErrors );
                }

                string cakeArg = cakeContext.Argument( argumentAttribute.ArgName, argumentAttribute.DefaultValue );
                if( argumentAttribute.Required && string.IsNullOrWhiteSpace( cakeArg ) )
                {
                    errors.AppendLine( "Argument not specified, but is required: " + argumentAttribute.ArgName );
                }

                info.SetValue(
                    instance,
                    cakeArg
                );
            }
        }

        if( errors.Length != 0 )
        {
            throw new InvalidOperationException(
                "Errors when validating arguments: " + Environment.NewLine + errors.ToString()
            );
        }

        return instance;
    }
}