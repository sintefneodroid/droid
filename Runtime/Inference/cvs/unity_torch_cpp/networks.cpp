#include <torch/script.h>
#include <vector>
#include <memory>

extern "C" { 	// This is going to store the loaded network
 	torch::jit::script::Module network;

 extern __attribute__((visibility("default"))) void InitNetwork() {
	  network = torch::jit::load("network_trace.pt");
		network.to(at::kCUDA); // If we're doing this on GPU
	}

 extern __attribute__((visibility("default"))) void ApplyNetwork(float *data, float *output) {
	 	auto x = torch::from_blob(data, {1,3,64,64}).cuda();
	 	std::vector<torch::jit::IValue> inputs;
		inputs.push_back(x);
		auto z = network.forward(inputs).toTensor();
			for (int i=0; i<1*5*64*64; i++){
				output[i] = z[0][i].item<float>();
		}
	}
}
