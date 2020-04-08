var url, qs, kv;

QueryString = function (key) {
	return window.location.toString().QueryString(key);
};

String.prototype.QueryString = function (key) {
	url = this.toString().toLowerCase();
	if (url.lastIndexOf('?') >= 0) {
		qs = url.substring(url.lastIndexOf('?') + 1).split('&');
		for (var i = 0; i < qs.length; i++) {
			kv = qs[i].split('=');
			if (kv[0] == key.toLowerCase()) {
				return kv[1];
			}
		}
	} else {
		return undefined;
	}
};


QueryStringCount = function () {
	return window.location.toString().QueryStringCount();
};

String.prototype.QueryStringCount = function () {
	url = this.toString();
	var items = new Array();
	if (url.lastIndexOf('?') >= 0) {
		qs = url.substring(url.lastIndexOf('?') + 1).split('&');
		if (qs == "") {
			return 0;
		} else {
			return qs.length;
		}
	} else {
		return 0;
	}
};
