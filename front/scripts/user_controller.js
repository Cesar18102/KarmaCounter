function getProfileById(id, callback) {
	let request = new XMLHttpRequest();
	request.open('GET', 'http://karmacountertwo.somee.com/api/user/get?id=' + id);
	request.onload = function() {
		if (request.status != 200)
			callback(false, request.response)
		else
			callback(true, request.response);
	}
	request.send();
}

function getUserActions(id, callback) {
	let request = new XMLHttpRequest();
	request.open('GET', 'http://karmacountertwo.somee.com/api/user/getactions?userId=' + id);
	request.onload = function() {
		if (request.status != 200)
			callback(false, request.response)
		else
			callback(true, request.response);
	}
	request.send();
}