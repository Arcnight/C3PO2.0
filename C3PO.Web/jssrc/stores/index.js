import { merge } from 'lodash';

import { authInitialState } from 'auth';
import { userInitialState } from 'user';

var appStore = {};

merge(appStore, authInitialState);
merge(appStore, userInitialState);

export default appStore;