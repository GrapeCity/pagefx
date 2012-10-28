/// <reference path="../core.js"/>

module("ConvertTests");

System = {};
System.Int64 = function(hi, lo) {
	this.m_hi = hi;
	this.m_lo = lo;
};
System.UInt64 = function(hi, lo) {
	this.m_hi = hi;
	this.m_lo = lo;
};

function check64(v, hi, lo) {
	equal(v.m_hi, hi);
	equal(v.m_lo, lo);
}

test("FromBoolean", function() {
	equal($conv(true, $tc.b, $tc.b), true);
	equal($conv(true, $tc.b, $tc.c), 1);
	equal($conv(true, $tc.b, $tc.i1), 1);
	equal($conv(true, $tc.b, $tc.u1), 1);
	equal($conv(true, $tc.b, $tc.i2), 1);
	equal($conv(true, $tc.b, $tc.u2), 1);
	equal($conv(true, $tc.b, $tc.i4), 1);
	equal($conv(true, $tc.b, $tc.u4), 1);
	check64($conv(true, $tc.b, $tc.i8), 0, 1);
	check64($conv(true, $tc.b, $tc.u8), 0, 1);
	equal($conv(true, $tc.b, $tc.r4), 1);
	equal($conv(true, $tc.b, $tc.r8), 1);
	equal($conv(true, $tc.b, $tc.s), "True");

	equal($conv(false, $tc.b, $tc.b), false);
	equal($conv(false, $tc.b, $tc.c), 0);
	equal($conv(false, $tc.b, $tc.i1), 0);
	equal($conv(false, $tc.b, $tc.u1), 0);
	equal($conv(false, $tc.b, $tc.i2), 0);
	equal($conv(false, $tc.b, $tc.u2), 0);
	equal($conv(false, $tc.b, $tc.i4), 0);
	equal($conv(false, $tc.b, $tc.u4), 0);
	check64($conv(false, $tc.b, $tc.i8), 0, 0);
	check64($conv(false, $tc.b, $tc.u8), 0, 0);
	equal($conv(false, $tc.b, $tc.r4), 0);
	equal($conv(false, $tc.b, $tc.r8), 0);
	equal($conv(false, $tc.b, $tc.s), "False");
});

test("FromSByte", function() {
	var min = -128;
	var max = 127;
	
	equal($conv(0, $tc.i1, $tc.b), false);
	equal($conv(1, $tc.i1, $tc.b), true);
	equal($conv(-1, $tc.i1, $tc.b), true);
	equal($conv(min, $tc.i1, $tc.b), true);
	equal($conv(max, $tc.i1, $tc.b), true); //5
	
	equal($conv(0, $tc.i1, $tc.i1), 0);
	equal($conv(1, $tc.i1, $tc.i1), 1);
	equal($conv(-1, $tc.i1, $tc.i1), -1);
	equal($conv(min, $tc.i1, $tc.i1), min);
	equal($conv(max, $tc.i1, $tc.i1), max); //10

	equal($conv(0, $tc.i1, $tc.u1), 0);
	equal($conv(1, $tc.i1, $tc.u1), 1);
	equal($conv(-1, $tc.i1, $tc.u1), 0xff);
	equal($conv(min, $tc.i1, $tc.u1), 0x80);
	equal($conv(max, $tc.i1, $tc.u1), max); //15

	equal($conv(0, $tc.i1, $tc.i2), 0);
	equal($conv(1, $tc.i1, $tc.i2), 1);
	equal($conv(-1, $tc.i1, $tc.i2), -1);
	equal($conv(min, $tc.i1, $tc.i2), min);
	equal($conv(max, $tc.i1, $tc.i2), max); //20

	equal($conv(0, $tc.i1, $tc.u2), 0);
	equal($conv(1, $tc.i1, $tc.u2), 1);
	equal($conv(-1, $tc.i1, $tc.u2), 0xffff);
	equal($conv(min, $tc.i1, $tc.u2), 0xff80);
	equal($conv(max, $tc.i1, $tc.u2), max); //25

	equal($conv(0, $tc.i1, $tc.i4), 0);
	equal($conv(1, $tc.i1, $tc.i4), 1);
	equal($conv(-1, $tc.i1, $tc.i4), -1);
	equal($conv(min, $tc.i1, $tc.i4), min);
	equal($conv(max, $tc.i1, $tc.i4), max); //30

	equal($conv(0, $tc.i1, $tc.u4), 0);
	equal($conv(1, $tc.i1, $tc.u4), 1);
	equal($conv(-1, $tc.i1, $tc.u4), 0xffffffff);
	equal($conv(min, $tc.i1, $tc.u4), 0xffffff80);
	equal($conv(max, $tc.i1, $tc.u4), 0x7f); //35

	check64($conv(0, $tc.i1, $tc.i8), 0, 0);
	check64($conv(1, $tc.i1, $tc.i8), 0, 1);
	check64($conv(-1, $tc.i1, $tc.i8), -1, 0xffffffff);
	check64($conv(min, $tc.i1, $tc.i8), -1, 0xffffff80);
	check64($conv(max, $tc.i1, $tc.i8), 0, max); //45

	check64($conv(0, $tc.i1, $tc.u8), 0, 0);
	check64($conv(1, $tc.i1, $tc.u8), 0, 1);
	check64($conv(-1, $tc.i1, $tc.u8), 0xffffffff, 0xffffffff);
	check64($conv(min, $tc.i1, $tc.u8), 0xffffffff, 0xffffff80);
	check64($conv(max, $tc.i1, $tc.u8), 0, max); //55

	equal($conv(0, $tc.i1, $tc.r4), 0);
	equal($conv(1, $tc.i1, $tc.r4), 1);
	equal($conv(-1, $tc.i1, $tc.r4), -1);
	equal($conv(min, $tc.i1, $tc.r4), min);
	equal($conv(max, $tc.i1, $tc.r4), max); //60

	equal($conv(0, $tc.i1, $tc.r8), 0);
	equal($conv(1, $tc.i1, $tc.r8), 1);
	equal($conv(-1, $tc.i1, $tc.r8), -1);
	equal($conv(min, $tc.i1, $tc.r8), min);
	equal($conv(max, $tc.i1, $tc.r8), max); //65

	equal($conv(0, $tc.i1, $tc.s), "0");
	equal($conv(1, $tc.i1, $tc.s), "1");
	equal($conv(-1, $tc.i1, $tc.s), "-1");
	equal($conv(min, $tc.i1, $tc.s), min.toString());
	equal($conv(max, $tc.i1, $tc.s), max.toString()); //70
});

test("FromByte", function() {
	var min = 0;
	var max = 0xff;

	equal($conv(min, $tc.u1, $tc.b), false);
	equal($conv(max, $tc.u1, $tc.b), true);

	equal($conv(min, $tc.u1, $tc.i1), min);
	equal($conv(max, $tc.u1, $tc.i1), -1);

	equal($conv(min, $tc.u1, $tc.u1), min);
	equal($conv(max, $tc.u1, $tc.u1), max);

	equal($conv(min, $tc.u1, $tc.i2), min);
	equal($conv(max, $tc.u1, $tc.i2), max);

	equal($conv(min, $tc.u1, $tc.u2), min);
	equal($conv(max, $tc.u1, $tc.u2), max);

	equal($conv(min, $tc.u1, $tc.i4), min);
	equal($conv(max, $tc.u1, $tc.i4), max);

	equal($conv(min, $tc.u1, $tc.u4), min);
	equal($conv(max, $tc.u1, $tc.u4), max);

	check64($conv(min, $tc.u1, $tc.i8), 0, min);
	check64($conv(max, $tc.u1, $tc.i8), 0, max);

	check64($conv(min, $tc.u1, $tc.u8), 0, min);
	check64($conv(max, $tc.u1, $tc.u8), 0, max);

	equal($conv(min, $tc.u1, $tc.r4), min);
	equal($conv(max, $tc.u1, $tc.r4), max);

	equal($conv(min, $tc.u1, $tc.r8), min);
	equal($conv(max, $tc.u1, $tc.r8), max);

	equal($conv(min, $tc.u1, $tc.s), min.toString());
	equal($conv(max, $tc.u1, $tc.s), max.toString());
});

test("FromInt16", function() {
	var min = -32768;
	var max = 32767;

	equal($conv(0, $tc.i2, $tc.b), false);
	equal($conv(1, $tc.i2, $tc.b), true);
	equal($conv(-1, $tc.i2, $tc.b), true);
	equal($conv(min, $tc.i2, $tc.b), true);
	equal($conv(max, $tc.i2, $tc.b), true); //5

	equal($conv(0, $tc.i2, $tc.i1), 0);
	equal($conv(1, $tc.i2, $tc.i1), 1);
	equal($conv(-1, $tc.i2, $tc.i1), -1);
	equal($conv(min, $tc.i2, $tc.i1), 0);
	equal($conv(max, $tc.i2, $tc.i1), -1); //10

	equal($conv(0, $tc.i2, $tc.u1), 0);
	equal($conv(1, $tc.i2, $tc.u1), 1);
	equal($conv(-1, $tc.i2, $tc.u1), 0xff);
	equal($conv(min, $tc.i2, $tc.u1), 0);
	equal($conv(max, $tc.i2, $tc.u1), 0xff); //15

	equal($conv(0, $tc.i2, $tc.i2), 0);
	equal($conv(1, $tc.i2, $tc.i2), 1);
	equal($conv(-1, $tc.i2, $tc.i2), -1);
	equal($conv(min, $tc.i2, $tc.i2), min);
	equal($conv(max, $tc.i2, $tc.i2), max); //20

	equal($conv(0, $tc.i2, $tc.u2), 0);
	equal($conv(1, $tc.i2, $tc.u2), 1);
	equal($conv(-1, $tc.i2, $tc.u2), 0xffff);
	equal($conv(min, $tc.i2, $tc.u2), 0x8000);
	equal($conv(max, $tc.i2, $tc.u2), 0x7fff); //25

	equal($conv(0, $tc.i2, $tc.i4), 0);
	equal($conv(1, $tc.i2, $tc.i4), 1);
	equal($conv(-1, $tc.i2, $tc.i4), -1);
	equal($conv(min, $tc.i2, $tc.i4), min);
	equal($conv(max, $tc.i2, $tc.i4), max); //30

	equal($conv(0, $tc.i2, $tc.u4), 0);
	equal($conv(1, $tc.i2, $tc.u4), 1);
	equal($conv(-1, $tc.i2, $tc.u4), 0xffffffff);
	equal($conv(min, $tc.i2, $tc.u4), 0xffff8000);
	equal($conv(max, $tc.i2, $tc.u4), 0x7fff); //35

	check64($conv(0, $tc.i2, $tc.i8), 0, 0);
	check64($conv(1, $tc.i2, $tc.i8), 0, 1);
	check64($conv(-1, $tc.i2, $tc.i8), -1, 0xffffffff);
	check64($conv(min, $tc.i2, $tc.i8), -1, 0xffff8000);
	check64($conv(max, $tc.i2, $tc.i8), 0, max); //45

	check64($conv(0, $tc.i2, $tc.u8), 0, 0);
	check64($conv(1, $tc.i2, $tc.u8), 0, 1);
	check64($conv(-1, $tc.i2, $tc.u8), 0xffffffff, 0xffffffff);
	check64($conv(min, $tc.i2, $tc.u8), 0xffffffff, 0xffff8000);
	check64($conv(max, $tc.i2, $tc.u8), 0, max); //55

	equal($conv(0, $tc.i2, $tc.r4), 0);
	equal($conv(1, $tc.i2, $tc.r4), 1);
	equal($conv(-1, $tc.i2, $tc.r4), -1);
	equal($conv(min, $tc.i2, $tc.r4), min);
	equal($conv(max, $tc.i2, $tc.r4), max); //60

	equal($conv(0, $tc.i2, $tc.r8), 0);
	equal($conv(1, $tc.i2, $tc.r8), 1);
	equal($conv(-1, $tc.i2, $tc.r8), -1);
	equal($conv(min, $tc.i2, $tc.r8), min);
	equal($conv(max, $tc.i2, $tc.r8), max); //65

	equal($conv(0, $tc.i2, $tc.s), "0");
	equal($conv(1, $tc.i2, $tc.s), "1");
	equal($conv(-1, $tc.i2, $tc.s), "-1");
	equal($conv(min, $tc.i2, $tc.s), min.toString());
	equal($conv(max, $tc.i2, $tc.s), max.toString()); //70
});

test("FromUInt16", function() {
	var min = 0;
	var max = 0xffff;

	equal($conv(min, $tc.u2, $tc.b), false);
	equal($conv(max, $tc.u2, $tc.b), true);

	equal($conv(min, $tc.u2, $tc.i1), min);
	equal($conv(max, $tc.u2, $tc.i1), -1);

	equal($conv(min, $tc.u2, $tc.u1), min);
	equal($conv(max, $tc.u2, $tc.u1), 0xff);

	equal($conv(min, $tc.u2, $tc.i2), min);
	equal($conv(max, $tc.u2, $tc.i2), -1);

	equal($conv(min, $tc.u2, $tc.u2), min);
	equal($conv(max, $tc.u2, $tc.u2), max);

	equal($conv(min, $tc.u2, $tc.i4), min);
	equal($conv(max, $tc.u2, $tc.i4), max);

	equal($conv(min, $tc.u2, $tc.u4), min);
	equal($conv(max, $tc.u2, $tc.u4), max);

	check64($conv(min, $tc.u2, $tc.i8), 0, min);
	check64($conv(max, $tc.u2, $tc.i8), 0, max);

	check64($conv(min, $tc.u2, $tc.u8), 0, min);
	check64($conv(max, $tc.u2, $tc.u8), 0, max);

	equal($conv(min, $tc.u2, $tc.r4), min);
	equal($conv(max, $tc.u2, $tc.r4), max);

	equal($conv(min, $tc.u2, $tc.r8), min);
	equal($conv(max, $tc.u2, $tc.r8), max);

	equal($conv(min, $tc.u2, $tc.s), min.toString());
	equal($conv(max, $tc.u2, $tc.s), max.toString());
});

test("FromInt32", function() {
	var min = -2147483648;
	var max = 2147483647;

	equal($conv(0, $tc.i4, $tc.b), false);
	equal($conv(1, $tc.i4, $tc.b), true);
	equal($conv(-1, $tc.i4, $tc.b), true);
	equal($conv(min, $tc.i4, $tc.b), true);
	equal($conv(max, $tc.i4, $tc.b), true); //5

	equal($conv(0, $tc.i4, $tc.i1), 0);
	equal($conv(1, $tc.i4, $tc.i1), 1);
	equal($conv(-1, $tc.i4, $tc.i1), -1);
	equal($conv(min, $tc.i4, $tc.i1), 0);
	equal($conv(max, $tc.i4, $tc.i1), -1); //10

	equal($conv(0, $tc.i4, $tc.u1), 0);
	equal($conv(1, $tc.i4, $tc.u1), 1);
	equal($conv(-1, $tc.i4, $tc.u1), 0xff);
	equal($conv(min, $tc.i4, $tc.u1), 0);
	equal($conv(max, $tc.i4, $tc.u1), 0xff); //15

	equal($conv(0, $tc.i4, $tc.i2), 0);
	equal($conv(1, $tc.i4, $tc.i2), 1);
	equal($conv(-1, $tc.i4, $tc.i2), -1);
	equal($conv(min, $tc.i4, $tc.i2), 0);
	equal($conv(max, $tc.i4, $tc.i2), -1); //20

	equal($conv(0, $tc.i4, $tc.u2), 0);
	equal($conv(1, $tc.i4, $tc.u2), 1);
	equal($conv(-1, $tc.i4, $tc.u2), 0xffff);
	equal($conv(min, $tc.i4, $tc.u2), 0);
	equal($conv(max, $tc.i4, $tc.u2), 0xffff); //25

	equal($conv(0, $tc.i4, $tc.i4), 0);
	equal($conv(1, $tc.i4, $tc.i4), 1);
	equal($conv(-1, $tc.i4, $tc.i4), -1);
	equal($conv(min, $tc.i4, $tc.i4), min);
	equal($conv(max, $tc.i4, $tc.i4), max); //30

	equal($conv(0, $tc.i4, $tc.u4), 0);
	equal($conv(1, $tc.i4, $tc.u4), 1);
	equal($conv(-1, $tc.i4, $tc.u4), 0xffffffff);
	equal($conv(min, $tc.i4, $tc.u4), 0x80000000);
	equal($conv(max, $tc.i4, $tc.u4), 0x7fffffff); //35

	check64($conv(0, $tc.i4, $tc.i8), 0, 0);
	check64($conv(1, $tc.i4, $tc.i8), 0, 1);
	check64($conv(-1, $tc.i4, $tc.i8), -1, 0xffffffff);
	check64($conv(min, $tc.i4, $tc.i8), -1, 0x80000000);
	check64($conv(max, $tc.i4, $tc.i8), 0, max); //45

	check64($conv(0, $tc.i4, $tc.u8), 0, 0);
	check64($conv(1, $tc.i4, $tc.u8), 0, 1);
	check64($conv(-1, $tc.i4, $tc.u8), 0xffffffff, 0xffffffff);
	check64($conv(min, $tc.i4, $tc.u8), 0xffffffff, 0x80000000);
	check64($conv(max, $tc.i4, $tc.u8), 0, 0x7fffffff); //55

	equal($conv(0, $tc.i4, $tc.r4), 0);
	equal($conv(1, $tc.i4, $tc.r4), 1);
	equal($conv(-1, $tc.i4, $tc.r4), -1);
	equal($conv(min, $tc.i4, $tc.r4), min);
	equal($conv(max, $tc.i4, $tc.r4), max); //60

	equal($conv(0, $tc.i4, $tc.r8), 0);
	equal($conv(1, $tc.i4, $tc.r8), 1);
	equal($conv(-1, $tc.i4, $tc.r8), -1);
	equal($conv(min, $tc.i4, $tc.r8), min);
	equal($conv(max, $tc.i4, $tc.r8), max); //65

	equal($conv(0, $tc.i4, $tc.s), "0");
	equal($conv(1, $tc.i4, $tc.s), "1");
	equal($conv(-1, $tc.i4, $tc.s), "-1");
	equal($conv(min, $tc.i4, $tc.s), min.toString());
	equal($conv(max, $tc.i4, $tc.s), max.toString()); //70
});

test("FromUInt32", function() {
	var min = 0;
	var max = 0xffffffff;

	equal($conv(min, $tc.u4, $tc.b), false);
	equal($conv(max, $tc.u4, $tc.b), true);

	equal($conv(min, $tc.u4, $tc.i1), min);
	equal($conv(max, $tc.u4, $tc.i1), -1);

	equal($conv(min, $tc.u4, $tc.u1), min);
	equal($conv(max, $tc.u4, $tc.u1), 0xff);

	equal($conv(min, $tc.u4, $tc.i2), min);
	equal($conv(max, $tc.u4, $tc.i2), -1);

	equal($conv(min, $tc.u4, $tc.u2), min);
	equal($conv(max, $tc.u4, $tc.u2), 0xffff);

	equal($conv(min, $tc.u4, $tc.i4), min);
	equal($conv(max, $tc.u4, $tc.i4), -1);

	equal($conv(min, $tc.u4, $tc.u4), min);
	equal($conv(max, $tc.u4, $tc.u4), max);

	check64($conv(min, $tc.u4, $tc.i8), 0, min);
	check64($conv(max, $tc.u4, $tc.i8), 0, max);

	check64($conv(min, $tc.u4, $tc.u8), 0, min);
	check64($conv(max, $tc.u4, $tc.u8), 0, max);

	equal($conv(min, $tc.u4, $tc.r4), min);
	equal($conv(max, $tc.u4, $tc.r4), max);

	equal($conv(min, $tc.u4, $tc.r8), min);
	equal($conv(max, $tc.u4, $tc.r8), max);

	equal($conv(min, $tc.u4, $tc.s), min.toString());
	equal($conv(max, $tc.u4, $tc.s), max.toString());
});

test("FromInt64", function() {
	var z = new System.Int64(0, 0);
	var p1 = new System.Int64(0, 1);
	var m1 = new System.Int64(0xffffffff, 0xffffffff);
	var min = new System.Int64(0x80000000, 0);
	var max = new System.Int64(0x7fffffff, 0xffffffff);

	equal($conv(z, $tc.i8, $tc.b), false);
	equal($conv(p1, $tc.i8, $tc.b), true);
	equal($conv(m1, $tc.i8, $tc.b), true);
	equal($conv(min, $tc.i8, $tc.b), true);
	equal($conv(max, $tc.i8, $tc.b), true); //5

	equal($conv(z, $tc.i8, $tc.i1), 0);
	equal($conv(p1, $tc.i8, $tc.i1), 1);
	equal($conv(m1, $tc.i8, $tc.i1), -1);
	equal($conv(min, $tc.i8, $tc.i1), 0);
	equal($conv(max, $tc.i8, $tc.i1), -1); //10

	equal($conv(z, $tc.i8, $tc.u1), 0);
	equal($conv(p1, $tc.i8, $tc.u1), 1);
	equal($conv(m1, $tc.i8, $tc.u1), 0xff);
	equal($conv(min, $tc.i8, $tc.u1), 0);
	equal($conv(max, $tc.i8, $tc.u1), 0xff); //15

	equal($conv(z, $tc.i8, $tc.i2), 0);
	equal($conv(p1, $tc.i8, $tc.i2), 1);
	equal($conv(m1, $tc.i8, $tc.i2), -1);
	equal($conv(min, $tc.i8, $tc.i2), 0);
	equal($conv(max, $tc.i8, $tc.i2), -1); //20

	equal($conv(z, $tc.i8, $tc.u2), 0);
	equal($conv(p1, $tc.i8, $tc.u2), 1);
	equal($conv(m1, $tc.i8, $tc.u2), 0xffff);
	equal($conv(min, $tc.i8, $tc.u2), 0);
	equal($conv(max, $tc.i8, $tc.u2), 0xffff); //25

	equal($conv(z, $tc.i8, $tc.i4), 0);
	equal($conv(p1, $tc.i8, $tc.i4), 1);
	equal($conv(m1, $tc.i8, $tc.i4), -1);
	equal($conv(min, $tc.i8, $tc.i4), 0);
	equal($conv(max, $tc.i8, $tc.i4), -1); //30

	equal($conv(z, $tc.i8, $tc.u4), 0);
	equal($conv(p1, $tc.i8, $tc.u4), 1);
	equal($conv(m1, $tc.i8, $tc.u4), 0xffffffff);
	equal($conv(min, $tc.i8, $tc.u4), 0);
	equal($conv(max, $tc.i8, $tc.u4), 0xffffffff); //35

	check64($conv(z, $tc.i8, $tc.i8), 0, 0);
	check64($conv(p1, $tc.i8, $tc.i8), 0, 1);
	check64($conv(m1, $tc.i8, $tc.i8), m1.m_hi, m1.m_lo);
	check64($conv(min, $tc.i8, $tc.i8), min.m_hi, min.m_lo);
	check64($conv(max, $tc.i8, $tc.i8), max.m_hi, max.m_lo); //45

	check64($conv(z, $tc.i8, $tc.u8), 0, 0);
	check64($conv(p1, $tc.i8, $tc.u8), 0, 1);
	check64($conv(m1, $tc.i8, $tc.u8), 0xffffffff, 0xffffffff);
	check64($conv(min, $tc.i8, $tc.u8), 0x80000000, 0x00000000);
	check64($conv(max, $tc.i8, $tc.u8), 0x7fffffff, 0xffffffff); //55

	equal($conv(z, $tc.i8, $tc.r4), 0); //56
	equal($conv(p1, $tc.i8, $tc.r4), 1); //57
	equal($conv(m1, $tc.i8, $tc.r4), -1); //58
	equal($conv(min, $tc.i8, $tc.r4), min);
	equal($conv(max, $tc.i8, $tc.r4), max); //60

	equal($conv(z, $tc.i8, $tc.r8), 0);
	equal($conv(p1, $tc.i8, $tc.r8), 1);
	equal($conv(m1, $tc.i8, $tc.r8), -1);
	equal($conv(min, $tc.i8, $tc.r8), -9223372036854775808);
	equal($conv(max, $tc.i8, $tc.r8), 9223372036854775807); //65
});

test("FromUInt64", function() {
	var min = new System.UInt64(0, 0);
	var max = new System.UInt64(0xffffffff, 0xffffffff);

	equal($conv(min, $tc.u8, $tc.b), false);
	equal($conv(max, $tc.u8, $tc.b), true); //2

	equal($conv(min, $tc.u8, $tc.i1), 0);
	equal($conv(max, $tc.u8, $tc.i1), -1); //4

	equal($conv(min, $tc.u8, $tc.u1), 0);
	equal($conv(max, $tc.u8, $tc.u1), 0xff); //6

	equal($conv(min, $tc.u8, $tc.i2), 0);
	equal($conv(max, $tc.u8, $tc.i2), -1); //8

	equal($conv(min, $tc.u8, $tc.u2), 0);
	equal($conv(max, $tc.u8, $tc.u2), 0xffff); //10

	equal($conv(min, $tc.u8, $tc.i4), 0);
	equal($conv(max, $tc.u8, $tc.i4), -1); //12

	equal($conv(min, $tc.u8, $tc.u4), 0);
	equal($conv(max, $tc.u8, $tc.u4), 0xffffffff); //14

	check64($conv(min, $tc.u8, $tc.i8), 0, 0); //15,16
	check64($conv(max, $tc.u8, $tc.i8), -1, 0xffffffff); //17,18

	check64($conv(min, $tc.u8, $tc.u8), 0, 0);
	check64($conv(max, $tc.u8, $tc.u8), 0xffffffff, 0xffffffff);

	equal($conv(min, $tc.u8, $tc.r4), 0);
	equal($conv(max, $tc.u8, $tc.r4), 0xffffffffffffffff);

	equal($conv(min, $tc.u8, $tc.r8), 0);
	equal($conv(max, $tc.u8, $tc.r8), 0xffffffffffffffff);
});