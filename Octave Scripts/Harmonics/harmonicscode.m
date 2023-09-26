% 1st time start 2nd time end 3rd amplitude 4th frequency 5th Sampling freq
[time,mag] = generatesin(0,1,220,50,32000);
% 0th magnitude of sin 1st time start 2nd time end 3rd amplitude 4th frequency 5th Sampling freq 6th harmonics number
new_mag = generateharmonics(mag,0,1,220,50,32000,10);
% 1st magnitude 2nd fundmental frequency 3rd FS 
[freqs,mags] = getparameters(new_mag,50,32000);