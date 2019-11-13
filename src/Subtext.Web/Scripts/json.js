/*
Copyright (c) 2005 JSON.org

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The Software shall be used for Good, not Evil.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

/*
The global object JSON contains two methods.

JSON.stringify(value) takes a JavaScript value and produces a JSON text.
The value must not be cyclical.

JSON.parse(text) takes a JSON text and produces a JavaScript value. It will
return false if there is an error.
*/
var JSON = function() {
    var m = {
        '\b': '\\b',
        '\t': '\\t',
        '\n': '\\n',
        '\f': '\\f',
        '\r': '\\r',
        '"': '\\"',
        '\\': '\\\\'
    },
        s = {
            'boolean': function(x) {
                return String(x);
            },
            number: function(x) {
                return isFinite(x) ? String(x) : 'null';
            },
            string: function(x) {
                if (/["\\\x00-\x1f]/.test(x)) {
                    x = x.replace(/([\x00-\x1f\\"])/g, function(a, b) {
                        var c = m[b];
                        if (c) {
                            return c;
                        }
                        c = b.charCodeAt();
                        return '\\u00' +
                            Math.floor(c / 16).toString(16) +
                            (c % 16).toString(16);
                    });
                }
                return '"' + x + '"';
            },
            object: function(x) {
                if (x) {
                    var a = [], b, f, i, l, v;
                    if (x instanceof Array) {
                        a[0] = '[';
                        l = x.length;
                        for (i = 0; i < l; i += 1) {
                            v = x[i];
                            f = s[typeof v];
                            if (f) {
                                v = f(v);
                                if (typeof v == 'string') {
                                    if (b) {
                                        a[a.length] = ',';
                                    }
                                    a[a.length] = v;
                                    b = true;
                                }
                            }
                        }
                        a[a.length] = ']';
                    } else if (x instanceof Date) {
                        function p(n) { return n < 10 ? '0' + n : n; };
                        var tz = x.getTimezoneOffset();
                        if (tz != 0) {
                            var tzh = Math.floor(Math.abs(tz) / 60);
                            var tzm = Math.abs(tz) % 60;
                            tz = (tz < 0 ? '+' : '-') + p(tzh) + ':' + p(tzm);
                        }
                        else {
                            tz = 'Z';
                        }
                        return '"' +
                                x.getFullYear() + '-' +
                                p(x.getMonth() + 1) + '-' +
                                p(x.getDate()) + 'T' +
                                p(x.getHours()) + ':' +
                                p(x.getMinutes()) + ':' +
                                p(x.getSeconds()) + tz + '"';
                    } else if (x instanceof Object) {
                        a[0] = '{';
                        for (i in x) {
                            v = x[i];
                            f = s[typeof v];
                            if (f) {
                                v = f(v);
                                if (typeof v == 'string') {
                                    if (b) {
                                        a[a.length] = ',';
                                    }
                                    a.push(s.string(i), ':', v);
                                    b = true;
                                }
                            }
                        }
                        a[a.length] = '}';
                    } else {
                        return;
                    }
                    return a.join('');
                }
                return 'null';
            }
        };
    return {
        copyright: '(c)2005 JSON.org',
        license: 'http://www.crockford.com/JSON/license.html',
        /*
        Stringify a JavaScript value, producing a JSON text.
        */
        stringify: function(v) {
            var f = s[typeof v];
            if (f) {
                v = f(v);
                if (typeof v == 'string') {
                    return v;
                }
            }
            return null;
        },
        /*
        Parse a JSON text, producing a JavaScript value.
        It returns false if there is a syntax error.
        */
        eval: function(text) {
            try {
                if (/^("(\\.|[^"\\\n\r])*?"|[,:{}\[\]0-9.\-+Eaeflnr-u \n\r\t])+?$/.test(text)) {
                    return eval('(' + text + ')');
                }
            } catch (e) {
            }
            throw new SyntaxError("eval");
        },

        parse: function(text) {
            var at = 0;
            var ch = ' ';

            function error(m) {
                throw {
                    name: 'JSONError',
                    message: m,
                    at: at - 1,
                    text: text
                };
            }

            function next() {
                ch = text.charAt(at);
                at += 1;
                return ch;
            }

            function white() {
                while (ch) {
                    if (ch <= ' ') {
                        next();
                    } else if (ch == '/') {
                        switch (next()) {
                            case '/':
                                while (next() && ch != '\n' && ch != '\r') { }
                                break;
                            case '*':
                                next();
                                for (; ; ) {
                                    if (ch) {
                                        if (ch == '*') {
                                            if (next() == '/') {
                                                next();
                                                break;
                                            }
                                        } else {
                                            next();
                                        }
                                    } else {
                                        error("Unterminated comment");
                                    }
                                }
                                break;
                            default:
                                error("Syntax error");
                        }
                    } else {
                        break;
                    }
                }
            }

            function string() {
                var i, s = '', t, u;

                if (ch == '"') {
                    outer: while (next()) {
                        if (ch == '"') {
                            next();
                            return s;
                        } else if (ch == '\\') {
                            switch (next()) {
                                case 'b':
                                    s += '\b';
                                    break;
                                case 'f':
                                    s += '\f';
                                    break;
                                case 'n':
                                    s += '\n';
                                    break;
                                case 'r':
                                    s += '\r';
                                    break;
                                case 't':
                                    s += '\t';
                                    break;
                                case 'u':
                                    u = 0;
                                    for (i = 0; i < 4; i += 1) {
                                        t = parseInt(next(), 16);
                                        if (!isFinite(t)) {
                                            break outer;
                                        }
                                        u = u * 16 + t;
                                    }
                                    s += String.fromCharCode(u);
                                    break;
                                default:
                                    s += ch;
                            }
                        } else {
                            s += ch;
                        }
                    }
                }
                error("Bad string");
            }

            function array() {
                var a = [];

                if (ch == '[') {
                    next();
                    white();
                    if (ch == ']') {
                        next();
                        return a;
                    }
                    while (ch) {
                        a.push(value());
                        white();
                        if (ch == ']') {
                            next();
                            return a;
                        } else if (ch != ',') {
                            break;
                        }
                        next();
                        white();
                    }
                }
                error("Bad array");
            }

            function object() {
                var k, o = {};

                if (ch == '{') {
                    next();
                    white();
                    if (ch == '}') {
                        next();
                        return o;
                    }
                    while (ch) {
                        k = string();
                        white();
                        if (ch != ':') {
                            break;
                        }
                        next();
                        o[k] = value();
                        white();
                        if (ch == '}') {
                            next();
                            return o;
                        } else if (ch != ',') {
                            break;
                        }
                        next();
                        white();
                    }
                }
                error("Bad object");
            }

            function number() {
                var n = '', v;
                if (ch == '-') {
                    n = '-';
                    next();
                }
                while (ch >= '0' && ch <= '9') {
                    n += ch;
                    next();
                }
                if (ch == '.') {
                    n += '.';
                    while (next() && ch >= '0' && ch <= '9') {
                        n += ch;
                    }
                }
                if (ch == 'e' || ch == 'E') {
                    n += 'e';
                    next();
                    if (ch == '-' || ch == '+') {
                        n += ch;
                        next();
                    }
                    while (ch >= '0' && ch <= '9') {
                        n += ch;
                        next();
                    }
                }
                v = +n;
                if (!isFinite(v)) {
                    ////error("Bad number");
                } else {
                    return v;
                }
            }

            function word() {
                switch (ch) {
                    case 't':
                        if (next() == 'r' && next() == 'u' && next() == 'e') {
                            next();
                            return true;
                        }
                        break;
                    case 'f':
                        if (next() == 'a' && next() == 'l' && next() == 's' &&
                                next() == 'e') {
                            next();
                            return false;
                        }
                        break;
                    case 'n':
                        if (next() == 'u' && next() == 'l' && next() == 'l') {
                            next();
                            return null;
                        }
                        break;
                }
                error("Syntax error");
            }

            function value() {
                white();
                switch (ch) {
                    case '{':
                        return object();
                    case '[':
                        return array();
                    case '"':
                        return string();
                    case '-':
                        return number();
                    default:
                        return ch >= '0' && ch <= '9' ? number() : word();
                }
            }

            return value();
        }
    };
} ();