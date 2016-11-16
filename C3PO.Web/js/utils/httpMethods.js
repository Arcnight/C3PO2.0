import axios from 'axios';

export function get(url, id)
{
	return axios.get(url, id);
};

export function put(url, formData)
{
	return axios.put(url, formData);
};

export function post(url, formData)
{
	return axios.post(url, formData);
};

export function del(url, id)
{
	return axios.delete(url, id);
};