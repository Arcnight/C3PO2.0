import axios from 'axios';

export function get(url, id)
{
	return axios.get(url, id);
};

export function put(url, data, config)
{
	return axios.put(url, data, config);
};

export function post(url, data, config)
{
    return axios.post(url, data, config);
};

export function del(url, id)
{
	return axios.delete(url, id);
};