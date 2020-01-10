#load "Includes.cake"

string target = Argument( "target", "Hello" );

Task( "Hello" )
.Does(
    () =>
    {
        Information( "Hello, World!" );
    }
);

RunTarget( target );