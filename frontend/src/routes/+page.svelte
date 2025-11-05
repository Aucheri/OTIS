<script>
	import Chatbox from '$lib/Chatbox.svelte';
</script>

<div class="h-screen w-screen bg-neutral-50">
	<div class="absolute flex items-center gap-4">
		<img src="Otis.png" class="w-12" alt="Otis Logo" />
		<h1 class="text-3xl">Otis</h1>
	</div>

	<div class="h-screen lg:left-1/2 lg:w-1/2 lg:translate-x-1/2">
		<Chatbox />
	</div>
</div>
