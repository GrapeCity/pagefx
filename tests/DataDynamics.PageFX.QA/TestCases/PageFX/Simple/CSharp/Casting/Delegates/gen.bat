@rem 123

rv /T:Foo1 /out:Foo1.cs Template.cs
rv /T:Foo2 /out:Foo2.cs Template.cs

rv /T:"Func<A,A>" /out:FuncAA.cs Template.cs
rv /T:"Func<A,B>" /out:FuncAB.cs Template.cs
rv /T:"Func<A,C>" /out:FuncAC.cs Template.cs
rv /T:"Func<A,I>" /out:FuncAI.cs Template.cs

rv /T:"Func<B,A>" /out:FuncBA.cs Template.cs
rv /T:"Func<B,B>" /out:FuncBB.cs Template.cs
rv /T:"Func<B,C>" /out:FuncBC.cs Template.cs
rv /T:"Func<B,I>" /out:FuncBI.cs Template.cs

rv /T:"Func<C,A>" /out:FuncCA.cs Template.cs
rv /T:"Func<C,B>" /out:FuncCB.cs Template.cs
rv /T:"Func<C,C>" /out:FuncCC.cs Template.cs
rv /T:"Func<C,I>" /out:FuncCI.cs Template.cs

rv /T:"Func<I,A>" /out:FuncIA.cs Template.cs
rv /T:"Func<I,B>" /out:FuncIB.cs Template.cs
rv /T:"Func<I,C>" /out:FuncIC.cs Template.cs
rv /T:"Func<I,I>" /out:FuncII.cs Template.cs

rv /T:"Action<A>" /out:ActionA.cs Template.cs
rv /T:"Action<B>" /out:ActionB.cs Template.cs
rv /T:"Action<C>" /out:ActionC.cs Template.cs
rv /T:"Action<I>" /out:ActionI.cs Template.cs