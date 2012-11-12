//http://james.padolsey.com/javascript/double-bitwise-not/

$types = {};

// System.TypeCode
$tc = {
	Empty: 0,
	o: 1,
	DBNull: 2,
	b: 3,	// Boolean
	c: 4,	// Char
	i1: 5,	// SByte
	u1: 6,	// Byte
	i2: 7,	// Int16
	u2: 8,	// UInt16
	i4: 9,	// Int32
	u4: 10,	// UInt32
	i8: 11,	// Int64
	u8: 12,	// UInt64
	r4: 13,	// Single
	r8: 14,	// Double
	d: 15,	// Decimal
	DateTime: 16,	// DateTime
	s: 18,	// String

	stringify: function (v) {
		switch (v) {
			case $tc.Empty: return "Empty";
			case $tc.o: return "Object";
			case $tc.DBNull: return "DBNull";
			case $tc.b: return "Boolean";
			case $tc.c: return "Char";
			case $tc.i1: return "SByte";
			case $tc.u1: return "Byte";
			case $tc.i2: return "Int16";
			case $tc.u2: return "UInt16";
			case $tc.i4: return "Int32";
			case $tc.u4: return "UInt32";
			case $tc.i8: return "Int64";
			case $tc.u8: return "UInt64";
			case $tc.r4: return "Single";
			case $tc.r8: return "Double";
			case $tc.d: return "Decimal";
			case $tc.DateTime: return "DateTime";
			case $tc.s: return "String";
			default: return v.toString();
		}
	}
};

function $inherit($this, $base) {
	var p;
	for (p in $base.prototype)
		if (typeof ($this.prototype[p]) == 'undefined' || $this.prototype[p] == Object.prototype[p])
			$this.prototype[p] = $base.prototype[p];
	for (p in $base)
		if (typeof ($this[p]) == 'undefined')
			$this[p] = $base[p];
}

function $isint64(v) {
	if (v) {
		var t = v.GetType().$typecode;
		return t == $tc.i8 || t == $tc.u8;
	}
	return false;
}

function $istrue(v) {
	if (v === null || v === undefined) {
		return false;
	}
	switch (typeof (v)) {
		case "boolean":
			return v;
		case "number":
			return v != 0;
		default:
			if ($isint64(v)) return v.m_hi && v.m_lo;
			return v !== null && v !== undefined;
	}
}

function $add(x, y) {
	return x.$add ? x.$add(y) : x + y;
}

function $sub(x, y) {
	return x.$sub ? x.$sub(y) : x - y;
}

function $mul(x, y) {
	return x.$mul ? x.$mul(y) : x * y;
}

function $div(x, y) {
	return x.$div ? x.$div(y) : x / y;
}

function $idiv(x, y) {
	return ~~(x / y);
}

function $rem(x, y) {
	return x.$rem ? x.$rem(y) : x % y;
}

function $irem(x, y) {
	return ~~(x % y);
}

function $and(x, y) {
	return x.$and ? x.$and(y) : x & y;
}

function $or(x, y) {
	return x.$or ? x.$or(y) : x | y;
}

function $xor(x, y) {
	return x.$xor ? x.$xor(y) : x ^ y;
}

function $eq(x, y) {
	return x.$eq ? x.$eq(y) : x == y;
}

function $ge(x, y) {
	return x.$ge ? x.$ge(y) : x >= y;
}

function $le(x, y) {
	return x.$le ? x.$le(y) : x <= y;
}

function $gt(x, y) {
	return x.$gt ? x.$gt(y) : x > y;
}

function $lt(x, y) {
	return x.$lt ? x.$lt(y) : x < y;
}

function $neg(x) {
	return x.$neg ? x.$neg() : -x;
}

function $not(x) {
	return x.$not ? x.$not() : ~x;
}

//TODO: move to System.Delegate as snippet
function $invokeDelegate(d, a, ret) {
	if (d.m_prev != null) {
		$invokeDelegate(d.m_prev, a, ret);
	}

	var val = d.m_function(d.m_target, a);
	
	return ret ? val : undefined;
}

function $copy(o) {
	if (o === null || o === undefined) return o;
	return o.$copy === undefined ? o : o.$copy();
}

// conditional boxing
function $cbox(o, box) {
	if (o === null || o === undefined) return o;
	if (o.$value === undefined) return box(o);
	return o;
}

function $unbox(o) {
	if (o === null || o === undefined) return o;
	var v = o.$value;
	return v === undefined ? o : v;
}

function $tostr(o) {
	if (o == null || o == undefined) return "";

	if (typeof (o) == "boolean")
		return o ? "True" : "False";
	
	return o.toString();
}

function $clone(o) {
	var c = o.GetType().$new();
	var fields = o.$fields();
	for (var i = 0; i < fields.length; i++) {
		var f = fields[i];
		c[f] = $copy(o[f]);
	}
	return c;
}

function $hashval(v) {
	switch (typeof(v)) {
		case "boolean":
			return v ? 1 : 0;
		case "number":
			//TODO: to long.GetHashCode
			return ~~v;
		case "string":
			return v.GetHashCode();
		default:
			return v ? v.toString().GetHashCode() : 0;
	}
}

function $hash(o) {
	var t = o.GetType();
	var h = t.ns.GetHashCode() ^ t.name.GetHashCode();
	var fields = o.$fields();
	for (var i = 0; i < fields.length; i++) {
		var f = fields[i];
		var v = o[f];
		if (v) {
			var ghc = v.GetHashCode;
			h ^= ghc === undefined ? $hashval(v) : ghc();
		}
	}
	return h;
}

// https://github.com/pgriess/node-jspack/blob/master/jspack.js
function $encodeFloat(a, p, v, el, bigEndian) {
	var s, e, m, i, d, c, mLen, eLen, eBias, eMax;
	mLen = el.mLen, eLen = el.len * 8 - el.mLen - 1, eMax = (1 << eLen) - 1, eBias = eMax >> 1;

	s = v < 0 ? 1 : 0;
	v = Math.abs(v);
	
	if (isNaN(v) || (v == Infinity)) {
		m = isNaN(v) ? 1 : 0;
		e = eMax;
	} else {
		e = Math.floor(Math.log(v) / Math.LN2); 		// Calculate log2 of the value
		if (v * (c = Math.pow(2, -e)) < 1) { e--; c *= 2; } 	// Math.log() isn't 100% reliable

		// Round by adding 1/2 the significand's LSD
		if (e + eBias >= 1) { v += el.rt / c; } 		// Normalized:  mLen significand digits
		else { v += el.rt * Math.pow(2, 1 - eBias); } 		// Denormalized:  <= mLen significand digits
		if (v * c >= 2) { e++; c /= 2; } 			// Rounding can increment the exponent

		if (e + eBias >= eMax) {
			// Overflow
			m = 0;
			e = eMax;
		}
		else if (e + eBias >= 1) {
			// Normalized - term order matters, as Math.pow(2, 52-e) and v*Math.pow(2, 52) can overflow
			m = (v * c - 1) * Math.pow(2, mLen);
			e = e + eBias;
		}
		else {
			// Denormalized - also catches the '0' case, somewhat by chance
			m = v * Math.pow(2, eBias - 1) * Math.pow(2, mLen);
			e = 0;
		}
	}

	for (i = bigEndian ? (el.len - 1) : 0, d = bigEndian ? -1 : 1; mLen >= 8; a[p + i] = m & 0xff, i += d, m /= 256, mLen -= 8);
	for (e = (e << mLen) | m, eLen += mLen; eLen > 0; a[p + i] = e & 0xff, i += d, e /= 256, eLen -= 8);
	a[p + i - d] |= s * 128;

	return a;
}

function $encodeSingle(v) {
	var a = new Array(4);
	return $encodeFloat(a, 0, v, {len: 4, mLen: 23, rt: Math.pow(2, -24) - Math.pow(2, -77)}, false);
}

function $encodeDouble(v) {
	var a = new Array(8);
	return $encodeFloat(a, 0, v, {len: 8, mLen: 52, rt: 0}, false);
}

// Derived from https://gist.github.com/2192799
function $decodeFloat(bytes, signBits, exponentBits, fractionBits, eMin, eMax, littleEndian) {
  // var totalBits = (signBits + exponentBits + fractionBits);

  var i;
  var binary = "";
  for (i = 0; i < bytes.length; i++) {
    var bits = bytes[i].toString(2);
    while (bits.length < 8) 
      bits = "0" + bits;

    if (littleEndian)
      binary = bits + binary;
    else
      binary += bits;
  }

  var sign = (binary.charAt(0) == '1')?-1:1;
  var exponent = parseInt(binary.substr(signBits, exponentBits), 2) - eMax;
  var significandBase = binary.substr(signBits + exponentBits, fractionBits);
  var significandBin = '1'+significandBase;
  i = 0;
  var val = 1;
  var significand = 0;

  if (exponent == -eMax) {
      if (significandBase.indexOf('1') == -1)
          return 0;
      else {
          exponent = eMin;
          significandBin = '0'+significandBase;
      }
  }

  while (i < significandBin.length) {
      significand += val * parseInt(significandBin.charAt(i));
      val = val / 2;
      i++;
  }

  return sign * significand * Math.pow(2, exponent);
}

function $decodeSingle(bytes) {
	return $decodeFloat(bytes, 1, 8, 23, -126, 127, true);
}

function $decodeDouble(bytes) {
	return $decodeFloat(bytes, 1, 11, 52, -1022, 1023, true);
}

// Substitutes "{n}" tokens within the specified string with the respective arguments passed in.
function $format(str) {

	if (!str) return '';

	for (var i = 1; i < arguments.length; i++) {
		str = str.replace(new RegExp("\\{" + (i - 1) + "\\}", "g"), arguments[i]);
	}

	return str;
}

function $conv(v, from, to) {

	if (from == to)
		return v;

	var n, hi, lo;

	switch (to) {
		case $tc.o: // to object
			return v;
		case $tc.b: // to boolean
			switch (from) {
				case $tc.b:
				case $tc.c:
				case $tc.i1:
				case $tc.u1:
				case $tc.i2:
				case $tc.u2:
				case $tc.i4:
				case $tc.u4:
				case $tc.r4:
				case $tc.r8:
					return v ? true : false;
				case $tc.i8:
				case $tc.u8:
					return v.m_hi || v.m_lo ? true : false;
				case $tc.s:
					v = v.toLower();
					if (v == "true") return true;
					if (v == "false") return false;
					n = parseFloat(v);
					if (!isNaN(n)) return n ? true : false;
					throw new Error("InvalidCastException");
			}
			break;
		case $tc.i1: // to sbyte
			switch (from) {
				case $tc.b:
					return v ? 1 : 0;
				case $tc.i1:
					return v;
				case $tc.c:
				case $tc.u1:
				case $tc.i2:
				case $tc.u2:
				case $tc.i4:
				case $tc.u4:
					n = v & 0xff;
					return n & 0x80 ? -((~n & 0xff) + 1) : n;
				case $tc.i8:
				case $tc.u8:
					n = v.m_lo & 0xff;
					return n & 0x80 ? -((~n & 0xff) + 1) : n;
				case $tc.r4:
				case $tc.r8:
					n = (~~v) & 0xff;
					return n & 0x80 ? -((~n & 0xff) + 1) : n;
				case $tc.s:
					n = parseFloat(v);
					if (isNaN(n)) throw new Error("InvalidCastException");
					n = (~~n) & 0xff;
					return n & 0x80 ? -((~n & 0xff) + 1) : n;
			}
			break;
		case $tc.u1: // to byte
			switch (from) {
				case $tc.b:
					return v ? 1 : 0;
				case $tc.i1:
				case $tc.c:
				case $tc.u1:
				case $tc.i2:
				case $tc.u2:
				case $tc.i4:
				case $tc.u4:
					return ((v >>> 0) & 0xff) >>> 0;
				case $tc.i8:
				case $tc.u8:
					return ((v.m_lo >>> 0) & 0xff) >>> 0;
				case $tc.r4:
				case $tc.r8:
					return (((~~v) >>> 0) & 0xff) >>> 0;
				case $tc.s:
					n = parseFloat(v);
					if (isNaN(n)) throw new Error("InvalidCastException");
					return (((~~n) >>> 0) & 0xff) >>> 0;
			}
			break;
		case $tc.i2: // to int16
			switch (from) {
				case $tc.b:
					return v ? 1 : 0;
				case $tc.i1:
				case $tc.u1:
				case $tc.i2:
					return v;
				case $tc.c:
				case $tc.u2:
				case $tc.i4:
				case $tc.u4:
					n = v & 0xffff;
					return n & 0x8000 ? -((~n & 0xffff) + 1) : n;
				case $tc.i8:
				case $tc.u8:
					n = v.m_lo & 0xffff;
					return n & 0x8000 ? -((~n & 0xffff) + 1) : n;
				case $tc.r4:
				case $tc.r8:
					n = (~~v) & 0xffff;
					return n & 0x8000 ? -((~n & 0xffff) + 1) : n;
				case $tc.s:
					n = parseFloat(v);
					if (isNaN(n)) throw new Error("InvalidCastException");
					n = (~~n) & 0xffff;
					return n & 0x8000 ? -((~n & 0xffff) + 1) : n;
			}
			break;
		case $tc.c: // to char
		case $tc.u2: // to uint16
			switch (from) {
				case $tc.b:
					return v ? 1 : 0;
				case $tc.u1:
				case $tc.c:
				case $tc.u2:
					return v;
				case $tc.i1:
				case $tc.i2:
				case $tc.i4:
				case $tc.u4:
					return ((v >>> 0) & 0xffff) >>> 0;
				case $tc.i8:
				case $tc.u8:
					return ((v.m_lo >>> 0) & 0xffff) >>> 0;
				case $tc.r4:
				case $tc.r8:
					return (((~~v) >>> 0) & 0xffff) >>> 0;
				case $tc.s:
					n = parseFloat(v);
					if (isNaN(n)) throw new Error("InvalidCastException");
					return (((~~n) >>> 0) & 0xffff) >>> 0;
			}
			break;
		case $tc.i4: // to int32
			switch (from) {
				case $tc.b:
					return v ? 1 : 0;
				case $tc.i1:
				case $tc.u1:
				case $tc.i2:
				case $tc.c:
				case $tc.u2:
				case $tc.i4:
					return v;
				case $tc.u4:
					n = v & 0xffffffff;
					return n & 0x80000000 ? -((~n & 0xffffffff) + 1) : n;
				case $tc.i8:
				case $tc.u8:
					n = v.m_lo & 0xffffffff;
					return n & 0x80000000 ? -((~n & 0xffffffff) + 1) : n;
				case $tc.r4:
				case $tc.r8:
					n = (~~v) & 0xffffffff;
					return n & 0x80000000 ? -((~n & 0xffffffff) + 1) : n;
				case $tc.s:
					n = parseFloat(v);
					if (isNaN(n)) throw new Error("InvalidCastException");
					n = (~~n) & 0xffffffff;
					return n & 0x80000000 ? -((~n & 0xffffffff) + 1) : n;
			}
			break;
		case $tc.u4: // to uint32
			switch (from) {
				case $tc.b:
					return v ? 1 : 0;
				case $tc.u1:
				case $tc.c:
				case $tc.u2:
				case $tc.u4:
					return v;
				case $tc.i1:
				case $tc.i2:
				case $tc.i4:
					return ((v >>> 0) & 0xffffffff) >>> 0;
				case $tc.i8:
				case $tc.u8:
					return ((v.m_lo >>> 0) & 0xffffffff) >>> 0;
				case $tc.r4:
				case $tc.r8:
					return (((~~v) >>> 0) & 0xffffffff) >>> 0;
				case $tc.s:
					n = parseFloat(v);
					if (isNaN(n)) throw new Error("InvalidCastException");
					return (((~~n) >>> 0) & 0xffffffff) >>> 0;
			}
			break;
		case $tc.i8: // to int64
			switch (from) {
				case $tc.b:
					return v ? new System.Int64(0, 1) : new System.Int64(0, 0);
				case $tc.u1:
				case $tc.c:
				case $tc.u2:
				case $tc.u4:
					return new System.Int64(0, v);
				case $tc.i1:
				case $tc.i2:
				case $tc.i4:
					n = ((v >>> 0) & 0xffffffff) >>> 0;
					return new System.Int64(v < 0 ? -1 : 0, n);
				case $tc.i8:
					return v;
				case $tc.u8:
					return new System.Int64(v.m_hi & 0xffffffff, v.m_lo);
				case $tc.r4:
				case $tc.r8:
					n = (((~~v) >>> 0) & 0xffffffff) >>> 0;
					return new System.Int64(v < 0 ? -1 : 0, n);
				case $tc.s:
					n = parseFloat(v);
					if (isNaN(n)) throw new Error("InvalidCastException");
					n = (((~~n) >>> 0) & 0xffffffff) >>> 0;
					return new System.Int64(v < 0 ? -1 : 0, n);
			}
			break;
		case $tc.u8: // to uint64
			switch (from) {
				case $tc.b:
					return v ? new System.UInt64(0, 1) : new System.UInt64(0, 0);
				case $tc.u1:
				case $tc.c:
				case $tc.u2:
				case $tc.u4:
					return new System.UInt64(0, v);
				case $tc.i1:
				case $tc.i2:
				case $tc.i4:
					n = ((v >>> 0) & 0xffffffff) >>> 0;
					return new System.UInt64(0, n);
				case $tc.i8:
					n = ((v.m_hi >>> 0) & 0xffffffff) >>> 0;
					return new System.UInt64(n, v.m_lo);
				case $tc.u8:
					return v;
				case $tc.r4:
				case $tc.r8:
					n = (((~~v) >>> 0) & 0xffffffff) >>> 0;
					return new System.UInt64(v < 0 ? 0xffffffff : 0, n);
				case $tc.s:
					n = parseFloat(v);
					if (isNaN(n)) throw new Error("InvalidCastException");
					hi = n < 0 ? 0xffffffff : 0;
					n = (((~~n) >>> 0) & 0xffffffff) >>> 0;
					return new System.UInt64(hi, n);
			}
			break;
		case $tc.r4: // to Single
		case $tc.r8: // to Double
			switch (from) {
				case $tc.b:
					return v ? 1 : 0;
				case $tc.c:
				case $tc.i1:
				case $tc.u1:
				case $tc.i2:
				case $tc.u2:
				case $tc.i4:
				case $tc.u4:
				case $tc.r4:
				case $tc.r8:
					return v;
				case $tc.i8:
					var sign = 1;
					hi = v.m_hi;
					lo = v.m_lo;
					if (hi & 0x80000000)
					{
						sign = -1;
						hi = ~hi & 0x7fffffff;
						lo = ~lo;
						if (!hi && !lo)
							return -1;
						return sign * (4294967296.0 * hi + lo);
					}
					return sign * (4294967296.0 * hi + lo);
				case $tc.u8:
					return 4294967296.0 * v.m_hi + v.m_lo;
				case $tc.s:
					n = parseFloat(v);
					if (isNaN(n)) throw new Error("InvalidCastException");
					return n;
			}
			break;
		case $tc.s: // to string
			switch (from) {
				case $tc.b:
					return v ? "True" : "False";
				case $tc.c:
				case $tc.i1:
				case $tc.u1:
				case $tc.i2:
				case $tc.u2:
				case $tc.i4:
				case $tc.u4:
				case $tc.r4:
				case $tc.r8:
				case $tc.i8:
				case $tc.u8:
				case $tc.s:
					return v.toString();
			}
			break;
	}

	throw new Error($format("$conv from {0} to {1} is not implemented!", $tc.stringify(from), $tc.stringify(to)));
}

function $convto(v, to) {
	var from = v.GetType().$typecode;
	if (from != to) {
		return $conv(v, from, to);
	}
	return v;
}

function $initarr(a, blob) {

	var arr = a.m_value;
	var e = a.$etc;

	var b1, b2, b3, b4, u, u2;
	var i = 0;
	var k = 0;
	while (k < blob.length && i < arr.length) {
		switch (e) {
			case $tc.b:
				arr[i++] = blob[k++] ? true : false;
				break;
			case $tc.i1:
				arr[i++] = blob[k++];
				break;
			case $tc.u1:
				arr[i++] = blob[k++];
				break;
			case $tc.i2:
				b1 = blob[k++];
				b2 = blob[k++];
				u = b1 | (b2 << 8);
				arr[i++] = u >> 15 ? ~~u : u;
				break;
			case $tc.c:
			case $tc.u2:
				b1 = blob[k++];
				b2 = blob[k++];
				arr[i++] = b1 | (b2 << 8);
				break;
			case $tc.i4:
				b1 = blob[k++];
				b2 = blob[k++];
				b3 = blob[k++];
				b4 = blob[k++];
				u = b1 | (b2 << 8) | (b3 << 16) | (b4 << 24);
				arr[i++] = $conv(u, $tc.u4, $tc.i4);
				break;
			case $tc.u4:
				b1 = blob[k++];
				b2 = blob[k++];
				b3 = blob[k++];
				b4 = blob[k++];
				arr[i++] = b1 | (b2 << 8) | (b3 << 16) | (b4 << 24);
				break;
			case $tc.i8:
				b1 = blob[k++];
				b2 = blob[k++];
				b3 = blob[k++];
				b4 = blob[k++];
				u = b1 | (b2 << 8) | (b3 << 16) | (b4 << 24);
				b1 = blob[k++];
				b2 = blob[k++];
				b3 = blob[k++];
				b4 = blob[k++];
				u2 = b1 | (b2 << 8) | (b3 << 16) | (b4 << 24);
				//TODO: $conv(new System.Int64(u2, u), $tc.u8, $tc.i8)?
				arr[i++] = new System.Int64(u2 & 0xffffffff, u);
				break;
			case $tc.u8:
				b1 = blob[k++];
				b2 = blob[k++];
				b3 = blob[k++];
				b4 = blob[k++];
				u = b1 | (b2 << 8) | (b3 << 16) | (b4 << 24);
				b1 = blob[k++];
				b2 = blob[k++];
				b3 = blob[k++];
				b4 = blob[k++];
				u2 = b1 | (b2 << 8) | (b3 << 16) | (b4 << 24);
				arr[i++] = new System.UInt64(u2, u);
				break;
			case $tc.r4:
				arr[i++] = $decodeSingle([blob[k++], blob[k++], blob[k++], blob[k++]]);
				break;
			case $tc.r8:
				arr[i++] = $decodeDouble([blob[k++], blob[k++], blob[k++], blob[k++], blob[k++], blob[k++], blob[k++], blob[k++]]);
				break;
			case /* Decimal */ $tc.d:
			case /* DateTime */ $tc.DateTime:
			case /* String */$tc.s:
				throw new Error("Not implemented!");
		}
	}
}

function $toSystemByteArray(nativeArray) {
	
	var arr = new System.Array();
	arr.m_rank = 1;
	arr.m_value = nativeArray;
	arr.m_type = "System.Byte[]";
	arr.m_box = function(v) {
		var o = new System.Byte();
		o.$value = v;
		return o;
	};
	
	arr.m_unbox = $unbox;
	arr.m_lbounds = null;
	arr.m_lengths = null;
	arr.m_dims = null;
	arr.$etc = 6; // element type code

	return arr;
}

var $isArray = Array.isArray || function(v) {
	return (typeof(v) == "object") && (v instanceof Array);
};

var $enum = {
	stringify: function (o) {
		var v = o.$value;
		var vals = o.$values();
		var s = this._find(v, vals);
		return s ? s : v.toString();
	},
	
	flags: function (o) {
		var v = o.$value;
		var vals = o.$values();
		var s = "";
		var $this = this;

		$this._process(vals,
			function (name, value) {

				var fname = $this._find(v, vals);
				if (fname) {
					if (s.length > 0) {
						s += ", ";
					}
					s += name;
					return true;
				}
				
				var f = $and(v, value);
				if ($istrue(f)) {
					v = $and(v, $not(value));					
					if (s.length > 0) {
						s += ", ";
					}
					s += name;
				}

				return false;
			});
		
		return s ? s : v.toString();
	},
	
	_find: function (v, vals) {
		if ($isArray(vals)) { // long enums
			for (var i = 0; i < vals.length; i++) {
				var p = vals[i];
				if ($eq(v, p.value)) {
					return p.name;
				}
			}
		} else if (v in vals) {
			return vals[v];
		}
		return null;
	},

	_process: function (vals, callback) {
		if ($isArray(vals)) {
			for (var i = 0; i < vals.length; i++) {
				var p = vals[i];
				if (callback(p.name, p.value))
					break;
			}
		} else {
			for (var key in vals) {
				if (callback(vals[key], parseInt(key)))
					break;
			}
		}
	}
};

function $context($method, $args, $vars) {

	var method = $method;
	var args = $args;
	var vars = $vars;
	var stack = [];
	var ip = 0; // instruction pointer
	var result;
	var exception;

	this.exec = function(code) {
		run(code);
		return result;
	};

	function push(v) {
		if (v === undefined) {
			throw new EvalError("undefined should not be pushed onto the eval stack.");
		}
		stack.push(v);
	}

	function pop(nocopy) {
		var o = stack.pop();
		return nocopy ? o : $copy(o);
	}

	// pops unsigned number
	function popun() {
		return pop(true);
	}

	function popptr() {
		//TODO: check ptr
		return pop(true);
	}

	function popobj() {
		var v = pop(true);
		if (v == null) return null;
		var f = v.$ptrget;
		return f === undefined ? v : f();
	}

	function peek() {
		return stack[stack.length - 1];
	}
	
	function run(code) {
		try {
			loop(code);
		} catch (e) {
			//find the most inner try block
			var b = findBlock();
			if (b === null) {
				throw e;
			}
			var h = findHandler(b, e, false);
			if (h === null) {
				throw e;
			}

			exception = e;
			stack = [e];
			ip = h.entry;
			
			run(code);
		}
	}
	
	function runFinally(code, bi) {
		var b = method.blocks[bi];
		var h = findHandler(b, null, true);

		exception = undefined;
		stack = [];
		ip = h.entry;

		run(code);
	}

	function findBlock() {
		var t = null;
		var blocks = method.blocks;
		for (var i = 0; i < blocks.length; i++) {
			var b = blocks[i];
			if (b.entry <= ip && ip <= b.exit) {
				if (t === null || b.entry >= t.entry) {
					t = b;
				}
			}
		}
		return t;
	}
	
	function findHandler(b, e, finallyFilter) {
		for (var i = 0; i < b.handlers.length; i++) {
			var h = b.handlers[i];
			
			if (finallyFilter) {
				if (h.type == 1) return h;
				continue;
			}
				
			switch (h.type) {
				case 0: //catch
					var et = h.exception;
					if (et === undefined || e instanceof et) {
						return h;
					}
					break;
				case 1: //finally
				case 2: //fault
					return h;
				case 3: //filter
					noimpl();
			}
		}
		return null;
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
				if (!$istrue(pop(true))) {
					ip = i[1];
					return;
				}
				break;
			case 45: // brtrue.s
			case 58: // brtrue
				if ($istrue(pop(true))) {
					ip = i[1];
					return;
				}
				break;
			case 46: // beq.s
			case 59: // beq
				y = pop(true);
				x = pop(true);
				if (eq(x, y, i[2])) {
					ip = i[1];
					return;
				}
				break;
			case 47: // bge.s
			case 60: // bge
				y = pop(true);
				x = pop(true);
				if (ge(x, y, i[2])) {
					ip = i[1];
					return;
				}
				break;
			case 48: // bgt.s
			case 61: // bgt
				y = pop(true);
				x = pop(true);
				if (gt(x, y, i[2])) {
					ip = i[1];
					return;
				}
				break;
			case 49: // ble.s
			case 62: // ble
				y = pop(true);
				x = pop(true);
				if (le(x, y, i[2])) {
					ip = i[1];
					return;
				}
				break;
			case 50: // blt.s
			case 63: // blt
				y = pop(true);
				x = pop(true);
				if (lt(x, y, i[2])) {
					ip = i[1];
					return;
				}
				break;
			case 51: // bne.un.s
			case 64: // bne.un
				y = popun();
				x = popun();
				if (!eq(x, y, i[2])) {
					ip = i[1];
					return;
				}
				break;
			case 52: // bge.un.s
			case 65: // bge.un
				y = popun();
				x = popun();
				if (ge(x, y, i[2])) {
					ip = i[1];
					return;
				}
				break;
			case 53: // bgt.un.s
			case 66: // bgt.un
				y = popun();
				x = popun();
				if (gt(x, y, i[2])) {
					ip = i[1];
					return;
				}
				break;
			case 54: // ble.un.s
			case 67: // ble.un
				y = popun();
				x = popun();
				if (le(x, y, i[2])) {
					ip = i[1];
					return;
				}
				break;
			case 55: // blt.un.s
			case 68: // blt.un
				y = popun();
				x = popun();
				if (lt(x, y, i[2])) {
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
				ldindt(i, $tc.i1);
				break;
			case 71: // ldind.u1
				ldindt(i, $tc.u1);
				break;
			case 72: // ldind.i2
				ldindt(i, $tc.i2);
				break;
			case 73: // ldind.u2
				ldindt(i, $tc.u2);
				break;
			case 74: // ldind.i4
				ldindt(i, $tc.i4);
				break;
			case 75: // ldind.u4
				ldindt(i, $tc.i4);
				break;
			case 76: // ldind.i8
				ldindt(i, $tc.i8);
				break;
			case 77: // ldind.i
				push(ldind()); // TODO: loads native int
				break;
			case 78: // ldind.r4
				ldindt(i, $tc.r4);
				break;
			case 79: // ldind.r8
				ldindt(i, $tc.r8);
				break;
			case 80: // ldind.ref
				push(ldind());
				break;
			case 81: // stind.ref
				stind();
				break;
			case 82: // stind.i1
			case 83: // stind.i2
			case 84: // stind.i4
			case 85: // stind.i8
			case 86: // stind.r4
			case 87: // stind.r8
				stindt(i);
				break;
			case 88: // add
				y = pop(true);
				x = pop(true);
				push(add(x, y, i[1]));
				break;
			case 89: // sub
				y = pop(true);
				x = pop(true);
				push(sub(x, y, i[1]));
				break;
			case 90: // mul
				y = pop(true);
				x = pop(true);
				push(mul(x, y, i[1]));
				break;
			case 91: // div
				y = pop(true);
				x = pop(true);
				push(div(x, y, i[1]));
				break;
			case 92: // div.un
				y = popun();
				x = popun();
				push(div(x, y, i[1]));
				break;
			case 93: // rem
				y = pop(true);
				x = pop(true);
				push(rem(x, y, i[1]));
				break;
			case 94: // rem.un
				y = popun();
				x = popun();
				push(rem(x, y, i[1]));
				break;
			case 95: // and
				y = pop(true);
				x = pop(true);
				push(and(x, y, i[1]));
				break;
			case 96: // or
				y = pop(true);
				x = pop(true);
				push(or(x, y, i[1]));
				break;
			case 97: // xor
				y = pop(true);
				x = pop(true);
				push(xor(x, y, i[1]));
				break;
			case 98: // shl
				y = pop(true);
				x = pop(true);
				push(shl(x, y, i[1]));
				break;
			case 99: // shr
				y = pop(true);
				x = pop(true);
				push(shr(x, y, i[1]));
				break;
			case 100: // shr.un
				y = popun();
				x = popun();
				push(shr(x, y, i[1]));
				break;
			case 101: // neg
				x = pop(true);
				push($neg(x));
				break;
			case 102: // not
				x = pop(true);
				push($not(x));
				break;
			case 103: // conv.i1
				convt(i, $tc.i1, false, false);
				break;
			case 104: // conv.i2
				convt(i, $tc.i2, false, false);
				break;
			case 105: // conv.i4
				convt(i, $tc.i4, false, false);
				break;
			case 106: // conv.i8
				convt(i, $tc.i8, false, false);
				break;
			case 107: // conv.r4
				convt(i, $tc.r4, false, false);
				break;
			case 108: // conv.r8
				convt(i, $tc.r8, false, false);
				break;
			case 109: // conv.u4
				convt(i, $tc.u4, false, false);
				break;
			case 110: // conv.u8
				convt(i, $tc.u8, false, false);
				break;
			case 111: // callvirt
				call(i[1], true);
				break;
			case 112: // cpobj
				break;
			case 113: // ldobj
				ldobj(i[1]);
				break;
			case 114: // ldstr
				push(i[1]);
				break;
			case 115: // newobj
				newobj(i[1]);
				break;
			case 116: // castclass
				x = popobj();
				push(cast(x, i[1]));
				break;
			case 117: // isinst
				x = popobj();
				push(isinst(x, i[1]));
				break;
			case 118: // conv.r.un
				push(convr4(popun(), i[1]));
				break;
			case 121: // unbox
				unbox(i[1]);
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
				stobj(i[1]);
				break;
			case 130: // conv.ovf.i1.un
				convt(i, $tc.i1, true, true);
				break;
			case 131: // conv.ovf.i2.un
				convt(i, $tc.i2, true, true);
				break;
			case 132: // conv.ovf.i4.un
				convt(i, $tc.i4, true, true);
				break;
			case 133: // conv.ovf.i8.un
				convt(i, $tc.i8, true, true);
				break;
			case 134: // conv.ovf.u1.un
				convt(i, $tc.u1, true, true);
				break;
			case 135: // conv.ovf.u2.un
				convt(i, $tc.u2, true, true);
				break;
			case 136: // conv.ovf.u4.un
				convt(i, $tc.u4, true, true);
				break;
			case 137: // conv.ovf.u8.un
				convt(i, $tc.u8, true, true);
				break;
			case 138: // conv.ovf.i.un
				noimpl();
				break;
			case 139: // conv.ovf.u.un
				noimpl();
				break;
			case 140: // box
				box(i[1]);
				break;
			case 141: // newarr
				push(newarr(pop(true), i[1]));
				break;
			case 142: // ldlen
				arr = poparr();
				push(arr.length);
				break;
			case 143: // ldelema
				index = pop(true);
				arr = poparr();
				push(new elemptr(arr, index));
				break;
			case 144: // ldelem.i1
				ldelemt(i, $tc.i1);
				break;
			case 145: // ldelem.u1
				ldelemt(i, $tc.u1);
				break;
			case 146: // ldelem.i2
				ldelemt(i, $tc.i2);
				break;
			case 147: // ldelem.u2
				ldelemt(i, $tc.u2);
				break;
			case 148: // ldelem.i4
				ldelemt(i, $tc.i4);
				break;
			case 149: // ldelem.u4
				ldelemt(i, $tc.u4);
				break;
			case 150: // ldelem.i8
				ldelemt(i, $tc.i8);
				break;
			case 151: // ldelem.i
				//TODO: native int
				push(ldelem());
				break;
			case 152: // ldelem.r4
				ldelemt(i, $tc.r4);
				break;
			case 153: // ldelem.r8
				ldelemt(i, $tc.r8);
				break;
			case 154: // ldelem.ref
				push(ldelem());
				break;
			case 155: // stelem.i
				stelem(); //TODO: native int
				break;
			case 156: // stelem.i1
				stelem(convi1, i[1]);
				break;
			case 157: // stelem.i2
				stelem(convi2, i[1]);
				break;
			case 158: // stelem.i4
				stelem(convi4, i[1]);
				break;
			case 159: // stelem.i8
				stelem(convi8, i[1]);
				break;
			case 160: // stelem.r4
				stelem(convr4, i[1]);
				break;
			case 161: // stelem.r8
				stelem(convr8, i[1]);
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
				unbox(i[1]);
				break;
			case 179: // conv.ovf.i1
				convt(i, $tc.i1, false, true);
				break;
			case 180: // conv.ovf.u1
				convt(i, $tc.u1, false, true);
				break;
			case 181: // conv.ovf.i2
				convt(i, $tc.i2, false, true);
				break;
			case 182: // conv.ovf.u2
				convt(i, $tc.u2, false, true);
				break;
			case 183: // conv.ovf.i4
				convt(i, $tc.i4, false, true);
				break;
			case 184: // conv.ovf.u4
				convt(i, $tc.u4, false, true);
				break;
			case 185: // conv.ovf.i8
				convt(i, $tc.i8, false, true);
				break;
			case 186: // conv.ovf.u8
				convt(i, $tc.u8, false, true);
				break;
			case 194: // refanyval
				break;
			case 195: // ckfinite
				break;
			case 198: // mkrefany
				break;
			case 208: // ldtoken
				push(i[1]);
				break;
			case 209: // conv.u2
				convt(i, $tc.u2, false, false);
				break;
			case 210: // conv.u1
				convt(i, $tc.u1, false, false);
				break;
			case 211: // conv.i
				noimpl();
				break;
			case 212: // conv.ovf.i
				noimpl();
				break;
			case 213: // conv.ovf.u
				noimpl();
				break;
			case 214: // add.ovf
				y = pop(true);
				x = pop(true);
				push(add(x, y, i[1]));
				break;
			case 215: // add.ovf.un
				y = popun();
				x = popun();
				push(add(x, y, i[1]));
				break;
			case 216: // mul.ovf
				y = pop(true);
				x = pop(true);
				push(mul(x, y, i[1]));
				break;
			case 217: // mul.ovf.un
				y = popun();
				x = popun();
				push(mul(x, y, i[1]));
				break;
			case 218: // sub.ovf
				y = pop(true);
				x = pop(true);
				push(sub(x, y, i[1]));
				break;
			case 219: // sub.ovf.un
				y = popun();
				x = popun();
				push(sub(x, y, i[1]));
				break;
			case 220: // endfinally
				ip = i[1];
				return;
			case 221: // leave
			case 222: // leave.s
				ip = i[1];
				var bi = i[2]; // index of try/finally block to run
				if (bi >= 0) {
					runFinally(code, bi);
				}
				return;
			case 223: // stind.
				noimpl();
				break;
			case 224: // conv.u
				noimpl();
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
				push(eq(x, y, i[1]));
				break;
			case -510: // cgt
				y = pop(true);
				x = pop(true);
				push(gt(x, y, i[1]));
				break;
			case -509: // cgt.un
				y = popun();
				x = popun();
				push(gt(x, y, i[1]));
				break;
			case -508: // clt
				y = pop(true);
				x = pop(true);
				push(lt(x, y, i[1]));
				break;
			case -507: // clt.un
				y = popun();
				x = popun();
				push(lt(x, y, i[1]));
				break;
			case -506: // ldftn
			case -505: // ldvirtftn
				ldftn(i[1]);
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

	function isinst(o, t) {
		if (o === null || o === undefined)
			return o;
		if (o.GetType().$hierarchy[t] == undefined)
			return null;
		return o;
	}

	function cast(o, t) {
		//TODO: casting primitive objects
		if (!isinst(o, t)) {
			isinst(o, t); // for debug
			throw new "InvalidCastException";
		}
		return o;
	}

	function box(f) {
		var o = popobj();
		var v = f(o);
		if (v === undefined) {
			v = f(o); // for debug
		}
		push(v);
	}

	function unbox(f) {
		box(f);
	}
	
	function popn(n) {
		var arr = [];
		for (var i = 0; i < n; i++) {
			var val = pop();
			arr.splice(0, 0, val);
		}
		return arr;
	}

	function newarr(n, e) {
		var v = [];
		for (var i = 0; i < n; i++) {
			v.push(e.init());
		}
		
		var arr = new System.Array();
		arr.m_rank = 1;
		arr.m_value = v;
		arr.m_type = e.type;
		arr.m_box = e.box;
		arr.m_unbox = e.unbox;
		arr.m_lbounds = null;
		arr.m_lengths = null;
		arr.m_dims = null;
		arr.$etc = e.etc; // element type code

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
			thisArg = popobj();
		}

		if (i.f === undefined) {
			throw new TypeError("func call is undefined!");
		}

		var val = i.f(thisArg, argArray);

		if (i.r) {

			// for debug
			if (val === undefined) {
				val = i.f(thisArg, argArray);
			}
			
			push(val);
		}
	}

	function calli() {
		noimpl();
	}
	
	function ldftn(f) {
		push(f);
	}

	// load/store fields
	function ldfld(f) {
		var o = popobj();
		var v = f.get(o);
		if (v === undefined) {
			v = f.get(o); // for debug
		}
		push(v);
	}

	function stfld(f) {
		var v = pop();
		var o = popobj();
		f.set(o, v);
	}

	function ldsfld(f) {
		var v = f.get(null);
		if (v === undefined) {
			v = f.get(null); // for debug
		}
		push(v);
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
		var v = pop(true);
		if (v.$ptrget) {
			return v.$ptrget();
		}
		return $unbox(v);
	}
	
	function ldindt(i, to) {
		var v = ldind();
		v = $conv(v, i[1], to);
		push(v);
	}

	function stind() {
		var v = pop();
		var p = popptr();
		p.$ptrset(v);
	}
	
	function stindt(i) {
		var v = pop();
		var p = popptr();
		var from = i[1];
		var to = i[2];
		if (from != to) {
			v = $conv(v, from, to);
		}
		p.$ptrset(v);
	}

	function ldobj(t) {
		var o = ldind();
		//TODO: cast!
		push(o);
	}
	
	function stobj(t) {
		var v = pop();
		var p = popptr();
		//TODO: cast!
		p.$ptrset(v);
	}
	
	function poparr() {
		var o = popobj();
		return o == null ? null : o.m_value;
	}
	
	function ldelem() {
		var i = pop(true);
		var arr = poparr();
		checkBounds(arr, i);
		return arr[i];
	}
	
	function ldelemt(i, to) {
		var v = ldelem();
		v = $conv(v, i[1], to);
		push(v);
	}
	
	function stelem(conv, from) {
		var value = pop();
		var i = pop(true);
		var arr = poparr();
		if (conv !== undefined) {
			value = conv(value, from);
		}
		checkBounds(arr, i);
		arr[i] = value;
	}

	function checkBounds(arr, index) {
		if (arr == null)
			throw new ReferenceError("NullReferenceException");
		if (index < 0 || index >= arr.length)
			throw new RangeError("IndexOutOfRangeException");
	}

	function isUnsigned(t) {
		switch (t) {
			case $tc.u1:
			case $tc.u2:
			case $tc.u4:
			case $tc.u8:
			case $tc.c:
				return true;
			default:
				return false;
		}
	}
	
	function bop(x, y, t, op) {
		var tx = (t >>> 16) & 0xff;
		var ty = (t >>> 8) & 0xff;
		var tr = Math.max(tx, ty);
		x = $conv(x, tx, tr);
		y = $conv(y, ty, tr);
		var v = op(x, y);
		return $conv(v, tr, t & 0xff);
	}
	
	function rel(x, y, t, op) {
		var tx = (t >>> 16) & 0xff;
		var ty = (t >>> 8) & 0xff;
		var tr = Math.max(tx, ty);
		x = $conv(x, tx, tr);
		y = $conv(y, ty, tr);
		return op(x, y);
	}

	function add(x, y, t) {
		return bop(x, y, t, $add);
	}

	function sub(x, y, t) {
		return bop(x, y, t, $sub);
	}

	function mul(x, y, t) {
		return bop(x, y, t, $mul);
	}

	function div(x, y, t) {
		if ((t & 0xff) <= $tc.u4) {
			return bop(x, y, t, $idiv);
		}
		return bop(x, y, t, $div);
	}

	function rem(x, y, t) {
		if ((t & 0xff) <= $tc.u4) {
			return bop(x, y, t, $irem);
		}
		return bop(x, y, t, $rem);
	}

	function and(x, y, t) {
		return bop(x, y, t, $and);
	}

	function or(x, y, t) {
		return bop(x, y, t, $or);
	}

	function xor(x, y, t) {
		return bop(x, y, t, $xor);
	}
	
	function shl(x, y, t) {
		// convert y to int32
		y = $conv(y, (t >>> 8) & 0xff, $tc.i4);
		if (x.$shl) return x.$shl(y);
		return $conv(x << y, $tc.i4, t & 0xff);
	}

	function shr(x, y, t) {
		// convert y to int32
		y = $conv(y, (t >>> 8) & 0xff, $tc.i4);
		if (x.$shr) return x.$shr(y);
		t = t & 0xff;
		var un = isUnsigned(t);
		return un ? $conv(x >>> y, $tc.u4, t) : $conv(x >> y, $tc.i4, t);
	}

	function eq(x, y, t) {
		if (x === null) return y === null;
		if (y === null) return false;
		return rel(x, y, t, $eq);
	}

	function ge(x, y, t) {
		return rel(x, y, t, $ge);
	}

	function le(x, y, t) {
		return rel(x, y, t, $le);
	}

	function gt(x, y, t) {
		return rel(x, y, t, $gt);
	}

	function lt(x, y, t) {
		return rel(x, y, t, $lt);
	}
	
	function convt(i, to, un, ovf) {
		var v = un ? popun() : pop(true);
		// TODO: implement conversion with overflow checking
		v = $conv(v, i[1], to);
		push(v);
	}
	
	function convi1(v, from) {
		return $conv(v, from, $tc.i1);
	}

	function convi2(v, from) {
		return $conv(v, from, $tc.i2);
	}

	function convi4(v, from) {
		return $conv(v, from, $tc.i4);
	}

	function convi8(v, from) {
		return $conv(v, from, $tc.i8);
	}

	function convr4(v, from) {
		return $conv(v, from, $tc.r4);
	}

	function convr8(v, from) {
		return $conv(v, from, $tc.r8);
	}
	
	function noimpl() {
		throw new Error("NotImplementedException");
	}

	function ignore(i) {
		//console.warn("ignored instruction %d", i[0]);
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

	function elemptr(a, i) {
		var arr = a;
		var index = i;
		this.$ptrget = function() {
			return arr[index];
		};
		this.$ptrset = function(v) {
			arr[index] = v;
			return v;
		};
	}
	
	function fldptr($o, $f) {
		var o = $o;
		var f = $f;
		this.$ptrget = function() {
			return f.get(o);
		};
		this.$ptrset = function(v) {
			f.set(o, v);
			return v;
		};
	}
}