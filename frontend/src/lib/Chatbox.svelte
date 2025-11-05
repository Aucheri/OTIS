<script lang="ts">
	import { marked } from 'marked';
	import { tick } from 'svelte';

	let message = $state('');
	let messageHistory: string[] = $state([]);
	let element: HTMLDivElement = $state();
	let inputBox: HTMLTextAreaElement = $state();
	let isSending = $state(false);

	async function SendData(event: SubmitEvent) {
		if (isSending) return;

		if (message.trim() === '') return;

		isSending = true;

		event.preventDefault();

		const m = message;

		messageHistory.push(message);
		message = '';

		await tick();

		ResizeInput();
		element.scroll({ top: element.scrollHeight, behavior: 'smooth' });
		const response = await fetch('http://localhost:5156/message', {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },

			body: JSON.stringify({ Message: m, Messages: messageHistory.slice(0, -1) })
		});

		messageHistory = await response.json();

		await tick();

		element.scroll({ top: element.scrollHeight, behavior: 'smooth' });

		isSending = false;
	}

	function input(ev: KeyboardEvent & { currentTarget: EventTarget & HTMLTextAreaElement }) {
		ResizeInput();

		console.log(ev);

		if (ev.key === 'Enter' && !ev.shiftKey) {
			ev.preventDefault?.();
			SendData(new Event('submit') as SubmitEvent);
		}
	}

	function ResizeInput() {
		inputBox.style.height = '';
		inputBox.style.height = inputBox.scrollHeight + 'px';
	}
</script>

<div class="grid h-full grid-rows-[1fr_auto] gap-4 p-10">
	{#if messageHistory.length <= 0}
		<div class="flex flex-col items-center justify-center">
			<h2 class="text-6xl font-bold">Chat to Otis</h2>
			<h3 class="text-3xl">Get help with your mental health!</h3>
		</div>
	{:else}
		<div class="flex flex-col gap-2 overflow-x-hidden overflow-y-auto" bind:this={element}>
			{#each messageHistory as message, i}
				<span
					class={`${i % 2 ? 'rounded-bl-none bg-neutral-600 text-white after:bg-neutral-600' : 'self-end rounded-br-none bg-neutral-200 after:bg-neutral-200'} relative w-fit max-w-3/4 rounded-md px-4 py-2 wrap-anywhere after:absolute after:right-0 after:bottom-0 after:block after:translate-y-1/2 ${i % 2 ? 'after:left-0 after:-translate-x-1/2' : 'after:translate-x-1/2'} after:aspect-square after:w-4 after:rotate-45`}
					>{@html marked(message)}</span
				>
			{/each}

			{#if isSending}
				<span
					class={`relative flex w-fit max-w-3/4 gap-2 rounded-md bg-neutral-600 px-4 py-2 after:absolute after:right-0 after:bottom-0 after:left-0 after:block after:aspect-square after:w-4 after:-translate-x-1/2 after:translate-y-1/2 after:rotate-45 after:bg-neutral-600`}
				>
					<div
						class="aspect-square w-3 animate-[bounce_1s_infinite_0ms] rounded-full bg-neutral-200"
					></div>
					<div
						class="aspect-square w-3 animate-[bounce_1s_infinite_50ms] rounded-full bg-neutral-200"
					></div>
					<div
						class="aspect-square w-3 animate-[bounce_1s_infinite_100ms] rounded-full bg-neutral-200"
					></div>
				</span>
			{/if}
		</div>
	{/if}

	<form
		onsubmit={SendData}
		class="relative left-1/2 flex h-auto max-h-36 min-h-12 -translate-x-1/2 items-end justify-between gap-4 rounded-2xl bg-neutral-800 p-2 shadow-2xl"
	>
		<textarea
			bind:value={message}
			bind:this={inputBox}
			onkeypress={input}
			class="h-12 max-h-full min-h-12 w-11/12 resize-none border-none bg-transparent wrap-anywhere text-white ring-0 outline-0 placeholder:text-neutral-300"
			placeholder="Ask for help with your mental health"
		></textarea>
		<button
			type="submit"
			disabled={isSending || message.length <= 0}
			class="h-10 cursor-pointer rounded-full bg-white p-2 disabled:cursor-not-allowed disabled:bg-neutral-500"
		>
			<img src="up-arrow.svg" alt="Arrow Up" class="z-10 aspect-square h-full" />
		</button>
	</form>
</div>
