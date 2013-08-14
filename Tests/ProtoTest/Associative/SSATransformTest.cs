using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using ProtoCore.DSASM.Mirror;
using ProtoCore.Lang;
using ProtoTest.TD;
using ProtoTestFx.TD;

namespace ProtoTest.Associative
{

    class SSATransformTest
    {
        public TestFrameWork thisTest = new TestFrameWork();
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void UpdateMember()
        {

            String code =
@"
class C
{
    x;
    constructor C()
    {
        x = 1;
    }
}

p = C.C();
a = p.x;
p.x = 10;
";
            ExecutionMirror mirror = thisTest.RunScriptSource(code);

            Obj o = mirror.GetValue("a");
            Assert.IsTrue((Int64)o.Payload == 10);
        }

        [Test]
        public void UpdateMemberArray1()
        {

            String code =
@"
class C
{
    x;
    constructor C()
    {
        x = {1,2,3};
    }
}

p = C.C();
a = p.x[2];
p.x = {10,20,30};
";
            ExecutionMirror mirror = thisTest.RunScriptSource(code);

            Obj o = mirror.GetValue("a");
            Assert.IsTrue((Int64)o.Payload == 30);
        }
    
        [Test]
        public void UpdateMemberArray2()
        {

            String code =
@"
class C
{
    x;
    constructor C()
    {
        x = {{1,2,3},{10,20,30}};
    }
}
i = 0;
j = 1;
p = C.C();
g = C.C();
a = p.x[i][j] + g.x[j][2];
g.x = {{1},{100,200,300,400}}; 
";
            ExecutionMirror mirror = thisTest.RunScriptSource(code);

            Obj o = mirror.GetValue("a");
            Assert.IsTrue((Int64)o.Payload == 302);
        }

        [Test]
        public void UpdateMemberArray3()
        {

            String code =
@"
class A
{
	x : var[];
	
	constructor A()
	{
		x = { B.B(), B.B(), B.B() };
	}
}

class B
{
	y : var[]..[];
	
	constructor B()
	{
		y = { { 1, 2 }, { 3, 4 }, { 5, 6 }};		
	}
}

a = { A.A(), A.A(), A.A() };

g = 2;
b = a[0].x[1].y[g][0]; 

a[0].x[1].y[g][0] = 100;
";
            ExecutionMirror mirror = thisTest.RunScriptSource(code);

            Obj o = mirror.GetValue("b");
            Assert.IsTrue((Int64)o.Payload == 100);
        }
    }
}