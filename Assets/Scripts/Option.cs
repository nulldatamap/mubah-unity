using System;

public class Option<T> {
  bool isSome;
  T inner;

  private Option() {
    isSome = false;
  }

  private Option( T i ) {
    isSome = true;
    inner = i;
  }


  public static Option<T> None() { return new Option<T>(); }
  public static Option<T> Some( T v ) { return new Option<T>( v ); }

  public bool IsSome() { return isSome; }
  public bool IsNone() { return !isSome; }

  public delegate V OnSome<V>( T s );
  public delegate V OnNone<V>();

  public V Match<V>( OnSome<V> onSome, OnNone<V> onNone ) {
    if( IsNone() ) {
      return onNone();
    }

    return onSome( inner );
  }

  public T Unwrap() {
    if( IsNone() ) {
      throw new InvalidOperationException( "Tried to unwrap None" );
    }

    return inner;
  }

  public T UnwrapOr( T o ) {
    return Match( x => x
                , () => o );
  }

  public delegate V ValueF<V>();

  public T UnwrapOrElse( ValueF<T> els ) {
    return Match( x => x
               , () => els() );
  }

  public delegate V MapF<V>( T t );

  public Option<V> Map<V>( MapF<V> mapf ) {
    return Match( x => Option<V>.Some( mapf( x ) )
                , () => Option<V>.None() );
  }

  public V MapOr<V>( V def, MapF<V> mapf ) {
    return Match( x => mapf( x )
                , () => def );
  }

  public V MapOrElse<V>( ValueF<V> def, MapF<V> mapf ) {
    return Match( x => mapf( x )
                , () => def() );
  }

  public Option<V> And<V>( Option<V> other ) {
    return Match( x => other
                , () => Option<V>.None() );
  }

  public delegate Option<V> Then<V>( T t );

  public Option<V> AndThen<V>( Then<V> then ) {
    return Match( x => then( x )
                , () => Option<V>.None() );
  }

  public Option<T> Or( Option<T> other ) {
    return Match( x => Some( x )
                , () => other );
  }

  public delegate Option<T> Else();

  public Option<T> OrElse( Else els ) {
    return Match( x => Some( x )
                , () => els() );
  }

  public delegate void Withf( T t );

  public void With( Withf with ) {
    if( IsSome() ) {
      with( inner );
    }
  }

}

