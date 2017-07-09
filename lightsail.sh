# Shell script to launch Sync server on Unbuntu 16.04 (tested on amazon lightsail)

sudo sh -c 'echo "deb [arch=amd64] https://apt-mo.trafficmanager.net/repos/dotnet-release/ xenial main" > /etc/apt/sources.list.d/dotnetdev.list' 
sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 417A0893 
sudo apt-get update 

yes Y | sudo apt-get install dotnet-dev-1.0.4 

# Cannot use ~ (home) under sudo
cd /home/ubuntu

# Downloading sources from git, need to create release for tag Stable
# for private repositories you have to use ssh public key to authenticate
wget https://github.com/Socialtalents/SocialTalents.Sync/archive/Stable.tar.gz 
tar -xzvf Stable.tar.gz 

chown -R ubuntu: SocialTalents.Sync-Stable
chmod -R u+w SocialTalents.Sync-Stable

runuser -l ubuntu -c 'cd SocialTalents.Sync-Stable/SocialTalents.SyncWeb && dotnet restore'
# todo: convert to agent supervisor
runuser -l ubuntu -c 'cd SocialTalents.Sync-Stable/SocialTalents.SyncWeb && sudo dotnet run -c Release'
