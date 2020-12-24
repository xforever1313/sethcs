#addin nuget:?package=Cake.ArgumentBinder

#load "DeleteHelpers.cake"

string target = Argument( "target", "default" );

Task("default")
.Does(
    () =>
    {
        Information( "Hello, World!" );
    }
);

RunTarget( target );