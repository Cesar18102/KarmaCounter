function getAllGroups(callback) {
	let request = new XMLHttpRequest();
	request.open('GET', 'http://karmacountertwo.somee.com/api/group/getall');
	request.onload = function() {
		if (request.status != 200)
			callback(false, request.response)
		else
			callback(true, request.response);
	}
	request.send();
}

function getGroupById(id, callback) {
	let request = new XMLHttpRequest();
	request.open('GET', 'http://karmacountertwo.somee.com/api/group/get?id=' + id);
	request.onload = function() {
		if (request.status != 200)
			callback(false, request.response)
		else
			callback(true, request.response);
	}
	request.send();
}

function getGroupActions(id, callback) {
	let request = new XMLHttpRequest();
	request.open('GET', 'http://karmacountertwo.somee.com/api/group/getactionsbygroup?groupId=' + id);
	request.onload = function() {
		if (request.status != 200)
			callback(false, request.response)
		else
			callback(true, request.response);
	}
	request.send();
}