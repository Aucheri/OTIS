<script>
	import { tick } from "svelte";

   
    let message = $state("")
    let messageHistory = $state([])

    async function SendData(){
        const m = message

        messageHistory.push(message)
        message = ""

        await tick()

        const response = await fetch("http://localhost:5156/message", {
            method: "POST",
            headers: {"Content-Type": "application/json"},

            body: JSON.stringify({Message:m, Messages:messageHistory.slice(0, -1)})
        })

        messageHistory = await response.json()
    }
</script>

<div class="flex flex-col overflow-y-auto w-1/2 border-2 p-10 rounded-2xl">
    {#each messageHistory as message, i}
        

		<span class={`${i % 2 ? "bg-red-500" : "bg-blue-500 self-end"} w-auto px-2 py-4 rounded-md max-w-1/2 wrap-anywhere relative after:block after:absolute after:right-0 after:bottom-0 after:w-2 ${i % 2 ? "after:bg-red-500 after:left-0" : "after:bg-blue-500"} after:aspect-square after:`}>{message}</span>
	{/each}
     
    <input bind:value={message}/>
    <button on:click={SendData}>Chat</button>
</div>