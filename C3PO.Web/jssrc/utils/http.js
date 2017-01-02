import axios from 'axios';
import appCache from 'store';

import { Assign } from 'Utils';
import { TOKEN_KEY } from 'Constants/keynames';

export function get(url, params, config)
{
    return axios.get(url, { params: params }, Assign(config, { auth: { 'Bearer': appCache[TOKEN_KEY] } }));
};

export function put(url, data, config)
{
    return axios.put(url, data, Assign(config, { auth: { 'Bearer': appCache[TOKEN_KEY] } }));
};

export function post(url, data, config)
{
    return axios.post(url, data, Assign(config, { auth: { 'Bearer': appCache[TOKEN_KEY] } }));
};

export function del(url, params, config)
{
    return axios.delete(url, { params: params }, Assign(config, { auth: { 'Bearer': appCache[TOKEN_KEY] } }));
};