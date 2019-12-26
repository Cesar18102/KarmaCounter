function findGetParameter(search, parameterName) {
    let result = null;
    let tmp = [];
    search.substr(1).split("&").forEach(
		function (item) {
			tmp = item.split("=");
			if (tmp[0] === parameterName) 
				result = decodeURIComponent(tmp[1]);
        }
	);
    return result;
}