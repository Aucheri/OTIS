
<script>
    import Chatbox from "$lib/Chatbox.svelte";
</script>

<div class="bg-neutral-50 h-screen w-screen">
	<div class="flex absolute items-center gap-4">
		<img src="Otis.png" class="w-12" alt="Otis Logo" />
		<h1 class="text-3xl">Otis</h1>
	</div>

	<div class="left-1/2 h-screen translate-x-1/2 w-1/2">
		<Chatbox />
	</div>
</div>