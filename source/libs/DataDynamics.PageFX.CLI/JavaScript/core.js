function $inherit($this, $base) {
	for (var p in $base.prototype)
		if (typeof ($this.prototype[p]) == 'undefined' || $this.prototype[p] == Object.prototype[p])
		$this.prototype[p] = $base.prototype[p];
	for (var p in $base)
		if (typeof ($this[p]) == 'undefined')
		$this[p] = $base[p];
}

function $context($method, $args, $vars) {

	var method = $method;
	var args = $args;
	var vars = $vars;
	var stack = [];
	var ip = 0; // instruction pointer
	var result;
	var exception;

	this.exec = function(code) {
		loop(code);
		return result;
	};

	function push(value) {
		stack.push(value);
	}

	function pop(nocopy) {
		return stack.pop();
	}
	
	// pops unsigned number
	function popun() {
		return pop();
	}

	function popptr() {
		//TODO: check ptr
		return pop(true);
	}

	function popobj() {
		var v = pop(true);
		var f = v.$ptrget;
		return f === undefined ? v : f();
	}

	function peek() {
		return stack[stack.length - 1];
	}
	
	function loop(code) {
		while (ip < code.length) {
			eval(code);
		}
	}

	function ldloc(index) {
		push(vars[index]);
	};

	function stloc(index, value) {
		vars[index] = value;
	};

	function ldarg(index) {
		push(args[index]);
	};

	function starg(index, value) {
		args[index] = value;
	};

	function eval(code) {
		var i = code[ip];
		var x, y, index, arr;
		switch (i[0]) {
			case 0: // nop
				break;
			case 1: // break
				break;
			case 2: // ldarg.0
				push(args[0]);
				break;
			case 3: // ldarg.1
				push(args[1]);
				break;
			case 4: // ldarg.2
				push(args[2]);
				break;
			case 5: // ldarg.3
				push(args[3]);
				break;
			case 6: // ldloc.0
				push(vars[0]);
				break;
			case 7: // ldloc.1
				push(vars[1]);
				break;
			case 8: // ldloc.2
				push(vars[2]);
				break;
			case 9: // ldloc.3
				push(vars[3]);
				break;
			case 10: // stloc.0
				vars[0] = pop();
				break;
			case 11: // stloc.1
				vars[1] = pop();
				break;
			case 12: // stloc.2
				vars[2] = pop();
				break;
			case 13: // stloc.3
				vars[3] = pop();
				break;
			case 14: // ldarg.s
				push(args[i[1]]);
				break;
			case 15: // ldarga.s
				push(new argptr(i[1]));
				break;
			case 16: // starg.s
				args[i[1]] = pop();
				break;
			case 17: // ldloc.s
				push(vars[i[1]]);
				break;
			case 18: // ldloca.s
				push(new varptr(i[1]));
				break;
			case 19: // stloc.s
				vars[i[1]] = pop();
				break;
			case 20: // ldnull
				push(null);
				break;
			case 21: // ldc.i4.m1
				push(-1);
				break;
			case 22: // ldc.i4.0
				push(0);
				break;
			case 23: // ldc.i4.1
				push(1);
				break;
			case 24: // ldc.i4.2
				push(2);
				break;
			case 25: // ldc.i4.3
				push(3);
				break;
			case 26: // ldc.i4.4
				push(4);
				break;
			case 27: // ldc.i4.5
				push(5);
				break;
			case 28: // ldc.i4.6
				push(6);
				break;
			case 29: // ldc.i4.7
				push(7);
				break;
			case 30: // ldc.i4.8
				push(8);
				break;
			case 31: // ldc.i4.s
			case 32: // ldc.i4
			case 34: // ldc.r4
			case 35: // ldc.r8
				push(i[1]);
				break;
			case 33: // ldc.i8
				//TODO: int64
				push(i[1]);
				break;
			case 37: // dup
				push(peek());
				break;
			case 38: // pop
				pop(true);
				break;
			case 40: // call
				call(i[1], false);
				break;
			case 41: // calli
				calli();
				break;
			case 42: // ret
				ip = code.length;
				if (!method.IsVoid) {
					result = pop();
				}
				return;
			case 43: // br.s
			case 56: // br
				ip = i[1];
				return;
			case 44: // brfalse.s
			case 57: // brfalse
				if (!istrue(pop(true))) {
					ip = i[1];
					return;
				}
				break;
			case 45: // brtrue.s
			case 58: // brtrue
				if (istrue(pop(true))) {
					ip = i[1];
					return;
				}
				break;
			case 46: // beq.s
			case 59: // beq
				y = pop(true);
				x = pop(true);
				if (eq(x, y)) {
					ip = i[1];
					return;
				}
				break;
			case 47: // bge.s
			case 60: // bge
				y = pop(true);
				x = pop(true);
				if (ge(x, y)) {
					ip = i[1];
					return;
				}
				break;
			case 48: // bgt.s
			case 61: // bgt
				y = pop(true);
				x = pop(true);
				if (gt(x, y)) {
					ip = i[1];
					return;
				}
				break;
			case 49: // ble.s
			case 62: // ble
				y = pop(true);
				x = pop(true);
				if (le(x, y)) {
					ip = i[1];
					return;
				}
				break;
			case 50: // blt.s
			case 63: // blt
				y = pop(true);
				x = pop(true);
				if (lt(x, y)) {
					ip = i[1];
					return;
				}
				break;
			case 51: // bne.un.s
			case 64: // bne.un
				y = popun();
				x = popun();
				if (!eq(x, y)) {
					ip = i[1];
					return;
				}
				break;
			case 52: // bge.un.s
			case 65: // bge.un
				y = popun();
				x = popun();
				if (ge(x, y)) {
					ip = i[1];
					return;
				}
				break;
			case 53: // bgt.un.s
			case 66: // bgt.un
				y = popun();
				x = popun();
				if (gt(x, y)) {
					ip = i[1];
					return;
				}
				break;
			case 54: // ble.un.s
			case 67: // ble.un
				y = popun();
				x = popun();
				if (le(x, y)) {
					ip = i[1];
					return;
				}
				break;
			case 55: // blt.un.s
			case 68: // blt.un
				y = popun();
				x = popun();
				if (lt(x, y)) {
					ip = i[1];
					return;
				}
				break;
			case 69: // switch
				index = pop();
				var targets = i[1];
				if (index >= 0 && index < targets.length) {
					ip = targets[index];
					return;
				}
				break;
			case 70: // ldind.i1
				push(convi1(ldind()));
				break;
			case 71: // ldind.u1
				push(convu2(ldind()));
				break;
			case 72: // ldind.i2
				push(convi2(ldind()));
				break;
			case 73: // ldind.u2
				push(convu2(ldind()));
				break;
			case 74: // ldind.i4
				push(convi4(ldind()));
				break;
			case 75: // ldind.u4
				push(convu4(ldind()));
				break;
			case 76: // ldind.i8
				push(convi8(ldind()));
				break;
			case 77: // ldind.i
				push(ldind()); // loads native int
				break;
			case 78: // ldind.r4
				push(convr4(ldind()));
				break;
			case 79: // ldind.r8
				push(convr8(ldind()));
				break;
			case 80: // ldind.ref
				push(ldind());
				break;
			case 81: // stind.ref
				stind();
				break;
			case 82: // stind.i1
				stind(convi1);
				break;
			case 83: // stind.i2
				stind(convi2);
				break;
			case 84: // stind.i4
				stind(convi4);
				break;
			case 85: // stind.i8
				stind(convi8);
				break;
			case 86: // stind.r4
				stind(convr4);
				break;
			case 87: // stind.r8
				stind(convr8);
				break;
			case 88: // add
				y = pop(true);
				x = pop(true);
				push(add(x, y));
				break;
			case 89: // sub
				y = pop(true);
				x = pop(true);
				push(sub(x, y));
				break;
			case 90: // mul
				y = pop(true);
				x = pop(true);
				push(mul(x, y));
				break;
			case 91: // div
				y = pop(true);
				x = pop(true);
				push(div(x, y));
				break;
			case 92: // div.un
				y = popun();
				x = popun();
				push(div(x, y));
				break;
			case 93: // rem
				y = pop(true);
				x = pop(true);
				push(rem(x, y));
				break;
			case 94: // rem.un
				y = popun();
				x = popun();
				push(rem(x, y));
				break;
			case 95: // and
				y = pop(true);
				x = pop(true);
				push(and(x, y));
				break;
			case 96: // or
				y = pop(true);
				x = pop(true);
				push(or(x, y));
				break;
			case 97: // xor
				y = pop(true);
				x = pop(true);
				push(xor(x, y));
				break;
			case 98: // shl
				y = pop(true);
				x = pop(true);
				push(shl(x, y));
				break;
			case 99: // shr
				y = pop(true);
				x = pop(true);
				push(shr(x, y));
				break;
			case 100: // shr.un
				y = popun();
				x = popun();
				push(shr(x, y));
				break;
			case 101: // neg
				x = pop(true);
				push(neg(x));
				break;
			case 102: // not
				x = pop(true);
				push(not(x));
				break;
			case 103: // conv.i1
				push(convi1(pop(true)));
				break;
			case 104: // conv.i2
				push(convi2(pop(true)));
				break;
			case 105: // conv.i4
				push(convi4(pop(true)));
				break;
			case 106: // conv.i8
				push(convi8(pop(true)));
				break;
			case 107: // conv.r4
				push(convr4(pop(true)));
				break;
			case 108: // conv.r8
				push(convr8(pop(true)));
				break;
			case 109: // conv.u4
				push(convu4(pop(true)));
				break;
			case 110: // conv.u8
				push(convu8(pop(true)));
				break;
			case 111: // callvirt
				call(i[1], true);
				break;
			case 112: // cpobj
				break;
			case 113: // ldobj
				break;
			case 114: // ldstr
				push(i[1]);
				break;
			case 115: // newobj
				newobj(i[1]);
				break;
			case 116: // castclass
				break;
			case 117: // isinst
				break;
			case 118: // conv.r.un
				push(convrun(pop(true)));
				break;
			case 121: // unbox
				break;
			case 122: // throw
				throw pop(true);
			case 123: // ldfld
				ldfld(i[1]);
				break;
			case 124: // ldflda
				ldflda(i[1]);
				break;
			case 125: // stfld
				stfld(i[1]);
				break;
			case 126: // ldsfld
				ldsfld(i[1]);
				break;
			case 127: // ldsflda
				ldsflda(i[1]);
				break;
			case 128: // stsfld
				stsfld(i[1]);
				break;
			case 129: // stobj
				break;
			case 130: // conv.ovf.i1.un
				push(convi1ovf(popun()));
				break;
			case 131: // conv.ovf.i2.un
				push(convi2ovf(popun()));
				break;
			case 132: // conv.ovf.i4.un
				push(convi4ovf(popun()));
				break;
			case 133: // conv.ovf.i8.un
				push(convi8ovf(popun()));
				break;
			case 134: // conv.ovf.u1.un
				push(convu1ovf(popun()));
				break;
			case 135: // conv.ovf.u2.un
				push(convu2ovf(popun()));
				break;
			case 136: // conv.ovf.u4.un
				push(convu4ovf(popun()));
				break;
			case 137: // conv.ovf.u8.un
				push(convu8ovf(popun()));
				break;
			case 138: // conv.ovf.i.un
				noimpl();
				break;
			case 139: // conv.ovf.u.un
				noimpl();
				break;
			case 140: // box
				break;
			case 141: // newarr
				break;
			case 142: // ldlen
				arr = popobj();
				push(arr.length);
				break;
			case 143: // ldelema
				index = pop(true);
				arr = popobj();
				push(new elemptr(arr, index));
				break;
			case 144: // ldelem.i1
				push(convi1(ldelem()));
				break;
			case 145: // ldelem.u1
				push(convu1(ldelem()));
				break;
			case 146: // ldelem.i2
				push(convi2(ldelem()));
				break;
			case 147: // ldelem.u2
				push(convu2(ldelem()));
				break;
			case 148: // ldelem.i4
				push(convi4(ldelem()));
				break;
			case 149: // ldelem.u4
				push(convu4(ldelem()));
				break;
			case 150: // ldelem.i8
				push(convi8(ldelem()));
				break;
			case 151: // ldelem.i
				//TODO: native int
				push(ldelem());
				break;
			case 152: // ldelem.r4
				push(convr4(ldelem()));
				break;
			case 153: // ldelem.r8
				push(convr8(ldelem()));
				break;
			case 154: // ldelem.ref
				push(ldelem());
				break;
			case 155: // stelem.i
				stelem();
				break;
			case 156: // stelem.i1
				stelem(convi1);
				break;
			case 157: // stelem.i2
				stelem(convi2);
				break;
			case 158: // stelem.i4
				stelem(convi4);
				break;
			case 159: // stelem.i8
				stelem(convi8);
				break;
			case 160: // stelem.r4
				stelem(convr4);
				break;
			case 161: // stelem.r8
				stelem(convr8);
				break;
			case 162: // stelem.ref
				stelem();
				break;
			case 163: // ldelem
				push(ldelem());
				break;
			case 164: // stelem
				stelem();
				break;
			case 165: // unbox.any
				break;
			case 179: // conv.ovf.i1
				push(convi1ovf(pop(true)));
				break;
			case 180: // conv.ovf.u1
				push(convu1ovf(pop(true)));
				break;
			case 181: // conv.ovf.i2
				push(convi2ovf(pop(true)));
				break;
			case 182: // conv.ovf.u2
				push(convu1ovf(pop(true)));
				break;
			case 183: // conv.ovf.i4
				push(convi4ovf(pop(true)));
				break;
			case 184: // conv.ovf.u4
				push(convu4ovf(pop(true)));
				break;
			case 185: // conv.ovf.i8
				push(convi8ovf(pop(true)));
				break;
			case 186: // conv.ovf.u8
				push(convu8ovf(pop(true)));
				break;
			case 194: // refanyval
				break;
			case 195: // ckfinite
				break;
			case 198: // mkrefany
				break;
			case 208: // ldtoken
				break;
			case 209: // conv.u2
				push(convu2(pop(true)));
				break;
			case 210: // conv.u1
				push(convu1(pop(true)));
				break;
			case 211: // conv.i
				push(convi(pop(true)));
				break;
			case 212: // conv.ovf.i
				push(conviovf(pop(true)));
				break;
			case 213: // conv.ovf.u
				push(convuovf(pop(true)));
				break;
			case 214: // add.ovf
				y = pop(true);
				x = pop(true);
				push(addovf(x, y));
				break;
			case 215: // add.ovf.un
				y = popun();
				x = popun();
				push(addovf(x, y));
				break;
			case 216: // mul.ovf
				y = pop(true);
				x = pop(true);
				push(mulovf(x, y));
				break;
			case 217: // mul.ovf.un
				y = popun();
				x = popun();
				push(mulovf(x, y));
				break;
			case 218: // sub.ovf
				y = pop(true);
				x = pop(true);
				push(subovf(x, y));
				break;
			case 219: // sub.ovf.un
				y = popun();
				x = popun();
				push(subovf(x, y));
				break;
			case 220: // endfinally
				break;
			case 221: // leave
			case 222: // leave.s
				ip = i[1];
				return;
			case 223: // stind.i
				break;
			case 224: // conv.u
				push(convu(pop(true)));
				break;
			case 248: // prefix7
			case 249: // prefix6
			case 250: // prefix5
			case 251: // prefix4
			case 252: // prefix3
			case 253: // prefix2
			case 254: // prefix1
			case 255: // prefixref
				ignore(i);
				break;
			case -512: // arglist
				noimpl();
				break;
			case -511: // ceq
				y = pop(true);
				x = pop(true);
				push(eq(x, y));
				break;
			case -510: // cgt
				y = pop(true);
				x = pop(true);
				push(gt(x, y));
				break;
			case -509: // cgt.un
				y = popun();
				x = popun();
				push(gt(x, y));
				break;
			case -508: // clt
				y = pop(true);
				x = pop(true);
				push(lt(x, y));
				break;
			case -507: // clt.un
				y = popun();
				x = popun();
				push(lt(x, y));
				break;
			case -506: // ldftn
				break;
			case -505: // ldvirtftn
				break;
			case -503: // ldarg
				ldarg(i[1]);
				break;
			case -502: // ldarga
				push(new argptr(i[1]));
				break;
			case -501: // starg
				starg(i[1], pop());
				break;
			case -500: // ldloc
				ldloc(i[1]);
				break;
			case -499: // ldloca
				push(new varptr(i[1]));
				break;
			case -498: // stloc
				stloc(i[1], pop());
				break;
			case -497: // localloc
				noimpl();
				break;
			case -495: // endfilter
				noimpl();
				break;
			case -494: // unaligned
				ignore(i);
				break;
			case -493: // volatile
				ignore(i);
				break;
			case -492: // tailcall
				noimpl();
				break;
			case -491: // initobj
				initobj(i[1]);
				break;
			case -490: // constrained
				ignore(i);
				break;
			case -489: // cpblk
				noimpl();
				break;
			case -488: // initblk
				noimpl();
				break;
			case -486: // rethrow
				throw exception;
			case -484: // sizeof
				break;
			case -483: // refanytype
				break;
			case -482: // readonly
				break;
		}
		
		ip++;
	}
	
	function popn(n) {
		var arr = [];
		for (var i = 0; i < n; i++) {
			var val = pop();
			arr.splice(0, 0, val);
		}
		return arr;
	}

	function newobj(i) {
		var argArray = popn(i.n);
		var obj = i.f(argArray);
		push(obj);
	}
	
	function initobj(i) {
		var p = popptr();
		p.$ptrset(i());
	}

	function call(i, virt) {
		var argArray = popn(i.n);
		
		//TODO: support getting properties
		
		var thisArg = null;
		if (!i.s) { // is not static
			thisArg = pop(true);
		}

		var val = i.f(thisArg).apply(thisArg, argArray);

		if (i.r) {
			push(val);
		}
	}

	function calli() {
		noimpl();
	}

	function ldfld(f) {
		push(f.get(popobj()));
	}
	
	function stfld(f) {
		var v = pop();
		var o = popobj();
		f.set(o, v);
	}

	function ldsfld(f) {
		push(f.get(null));
	}
	
	function stsfld(f) {
		var v = pop();
		f.set(null, v);
	}

	function ldflda(f) {
		var obj = popobj();
		push(new fldptr(obj, f));
	}

	function ldsflda(f) {
		push(new fldptr(null, f));
	}

	function ldind() {
		var ptr = popptr();
		return ptr.$ptrget();
	}
	
	function stind(conv) {
		var value = pop();
		var ptr = popptr();
		if (conv !== undefined) {
			value = conv(value);
		}
		ptr.$ptrset(value);
	}
	
	function ldelem() {
		var i = pop(true);
		var arr = popobj();
		checkBounds(arr, i);
		push(arr[i]);
	}
	
	function stelem(conv) {
		var value = pop();
		var i = pop(true);
		var arr = popobj();
		if (conv !== undefined) {
			value = conv(value);
		}
		checkBounds(arr, i);
		arr[i] = value;
	}
	
	function checkBounds(arr, index) {
		if (index < 0 || index >= arr.length)
			throw "IndexOutOfRangeException";
	}

	function istrue(v) {
		if (v === null || v === undefined) {
			return false;
		}
		switch (typeof (v)) {
			case "boolean":
				return v;
			case "number":
				return v != 0;
			default:
				//TODO: int64
				return v !== null && v !== undefined;
		}
	}
	
	function add(x, y) {
		//TODO: int64
		return x + y;
	}

	function addovf(x, y) {
		//TODO: int64
		return x + y;
	}

	function sub(x, y) {
		//TODO: int64
		return x - y;
	}

	function subovf(x, y) {
		//TODO: int64
		return x - y;
	}

	function mul(x, y) {
		//TODO: int64
		return x * y;
	}

	function mulovf(x, y) {
		//TODO: int64
		return x * y;
	}

	function div(x, y) {
		//TODO: int64
		return x / y;
	}

	function rem(x, y) {
		//TODO: int64
		return x % y;
	}

	function and(x, y) {
		//TODO: int64
		return x & y;
	}

	function or(x, y) {
		//TODO: int64
		return x | y;
	}

	function xor(x, y) {
		//TODO: int64
		return x ^ y;
	}

	function shl(x, y) {
		//TODO: int64
		return x << y;
	}

	function shr(x, y) {
		//TODO: int64
		return x >> y;
	}

	function neg(x) {
		//TODO: int64
		return -x;
	}
	
	function not(x) {
		//TODO: handle null?
		return !x;
	}

	function eq(x, y) {
		//TODO: int64
		return x == y;
	}

	function ge(x, y) {
		//TODO: int64
		return x >= y;
	}

	function le(x, y) {
		//TODO: int64
		return x <= y;
	}

	function gt(x, y) {
		//TODO: int64
		return x > y;
	}

	function lt(x, y) {
		//TODO: int64
		return x < y;
	}

	function convi1(v) {
		//TODO: int64
		return v & 0xff;
	}

	function convu1(v) {
		//TODO: int64
		return v & 0xff;
	}

	function convi2(v) {
		//TODO: int64
		return v & 0xffff;
	}

	function convu2(v) {
		//TODO: int64
		return v & 0xffff;
	}

	function convi4(v) {
		//TODO: int64
		return v & 0xffffffff;
	}

	function convu4(v) {
		//TODO: int64
		return v & 0xffffffff;
	}

	function convi8(v) {
		//TODO: int64
		return v & 0xffffffff;
	}

	function convu8(v) {
		//TODO: int64
		return v & 0xffffffff;
	}

	function convr4(v) {
		//TODO: int64
		return v;
	}

	function convr8(v) {
		//TODO: int64
		return v;
	}
	
	function noimpl() {
		throw "NotImplementedException";
	}

	function ignore(i) {
		console.warn("ignored instruction %d", i[0]);
	}

	function varptr(i) {
		var index = i;
		this.$ptrget = function() {
			return vars[index];
		};
		this.$ptrset = function(v) {
			stloc(index, v);
			return v;
		};
	}

	function argptr(i) {
		var index = i;
		this.$ptrget = function() {
			return args[index];
		};
		this.$ptrset = function(v) {
			starg(index, v);
			return v;
		};
	}

	function elemptr(arr, index) {
		this.arr = arr;
		this.index = index;
		this.$ptrget = function() {
			return this.arr[this.index];
		};
		this.$ptrset = function(v) {
			this.arr[this.index] = v;
			return v;
		};
	}
	
	function fldptr(o, f) {
		this.o = o;
		this.f = f;
		this.$ptrget = function() {
			return this.f.get(this.o);
		};
		this.$ptrset = function(v) {
			this.f.set(this.o, v);
			return v;
		};
	}
}